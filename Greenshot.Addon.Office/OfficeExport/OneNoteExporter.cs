﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2016 Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on GitHub: https://github.com/greenshot
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Greenshot.Addon.Configuration;
using Greenshot.Addon.Core;
using Greenshot.Addon.Interfaces;
using Greenshot.Addon.Interfaces.Plugin;
using OneNote = Microsoft.Office.Interop.OneNote;
using Dapplo.Log;

namespace Greenshot.Addon.Office.OfficeExport
{
	/// <summary>
	/// OneNote exporter
	/// More details about OneNote: http://msdn.microsoft.com/en-us/magazine/ff796230.aspx
	/// </summary>
	public class OneNoteExporter
	{
		private static readonly LogSource Log = new LogSource();
		private const string XmlImageContent = "<one:Image format=\"png\"><one:Size width=\"{1}.0\" height=\"{2}.0\" isSetByUser=\"true\" /><one:Data>{0}</one:Data></one:Image>";
		private const string XmlOutline = "<?xml version=\"1.0\"?><one:Page xmlns:one=\"{2}\" ID=\"{1}\"><one:Title><one:OE><one:T><![CDATA[{3}]]></one:T></one:OE></one:Title>{0}</one:Page>";
		private const string OnenoteNamespace2010 = "http://schemas.microsoft.com/office/onenote/2010/onenote";

		/// <summary>
		/// Create a new page in the "unfiled notes section", with the title of the capture, and export the capture there.
		/// </summary>
		/// <param name="surfaceToUpload">ISurface</param>
		/// <returns>bool true if export worked</returns>
		public static bool ExportToNewPage(ICapture surfaceToUpload)
		{
			using (var oneNoteApplication = GetOrCreateOneNoteApplication())
			{
				var newPage = new OneNotePage();
				string unfiledNotesSectionId = GetSectionId(oneNoteApplication, OneNote.SpecialLocation.slUnfiledNotesSection);
				if (unfiledNotesSectionId != null)
				{
					string pageId;
					oneNoteApplication.ComObject.CreateNewPage(unfiledNotesSectionId, out pageId, OneNote.NewPageStyle.npsDefault);
					newPage.Id = pageId;
					// Set the new name, this is automatically done in the export to page
					newPage.Name = surfaceToUpload.CaptureDetails.Title;
					return ExportToPage(oneNoteApplication, surfaceToUpload, newPage);
				}
			}
			return false;
		}

		/// <summary>
		/// Export the capture to the specified page
		/// </summary>
		/// <param name="surfaceToUpload">ISurface</param>
		/// <param name="page">OneNotePage</param>
		/// <returns>bool true if everything worked</returns>
		public static bool ExportToPage(ICapture surfaceToUpload, OneNotePage page)
		{
			using (var oneNoteApplication = GetOrCreateOneNoteApplication())
			{
				return ExportToPage(oneNoteApplication, surfaceToUpload, page);
			}
		}

		/// <summary>
		/// Export the capture to the specified page
		/// </summary>
		/// <param name="oneNoteApplication">IOneNoteApplication</param>
		/// <param name="surfaceToUpload">ISurface</param>
		/// <param name="page">OneNotePage</param>
		/// <returns>bool true if everything worked</returns>
		private static bool ExportToPage(IDisposableCom<OneNote.Application> oneNoteApplication, ICapture surfaceToUpload, OneNotePage page)
		{
			if (oneNoteApplication == null)
			{
				return false;
			}
			using (var pngStream = new MemoryStream())
			{
				var pngOutputSettings = new SurfaceOutputSettings(OutputFormat.png, 100, false);
				ImageOutput.SaveToStream(surfaceToUpload, pngStream, pngOutputSettings);
				string base64String = Convert.ToBase64String(pngStream.GetBuffer());
				string imageXmlStr = string.Format(XmlImageContent, base64String, surfaceToUpload.Image.Width, surfaceToUpload.Image.Height);
				string pageChangesXml = string.Format(XmlOutline, new object[]
				{
					imageXmlStr, page.Id, OnenoteNamespace2010, page.Name
				});
				Log.Info().WriteLine("Sending XML: {0}", pageChangesXml);
				oneNoteApplication.ComObject.UpdatePageContent(pageChangesXml, DateTime.MinValue, OneNote.XMLSchema.xs2010, false);
				try
				{
					oneNoteApplication.ComObject.NavigateTo(page.Id, null, false);
				}
				catch (Exception ex)
				{
					Log.Warn().WriteLine(ex, "Unable to navigate to the target page");
				}
				return true;
			}
		}

		/// <summary>
		/// Retrieve the Section ID for the specified special location
		/// </summary>
		/// <param name="oneNoteApplication"></param>
		/// <param name="specialLocation">SpecialLocation</param>
		/// <returns>string with section ID</returns>
		private static string GetSectionId(IDisposableCom<OneNote.Application> oneNoteApplication, OneNote.SpecialLocation specialLocation)
		{
			if (oneNoteApplication == null)
			{
				return null;
			}
			// ReSharper disable once RedundantAssignment
			string unfiledNotesPath = "";
			oneNoteApplication.ComObject.GetSpecialLocation(specialLocation, out unfiledNotesPath);

			// ReSharper disable once RedundantAssignment
			string notebookXml = "";
			oneNoteApplication.ComObject.GetHierarchy("", OneNote.HierarchyScope.hsPages, out notebookXml, OneNote.XMLSchema.xs2010);
			if (!string.IsNullOrEmpty(notebookXml))
			{
				Log.Debug().WriteLine(notebookXml);
				StringReader reader = null;
				try
				{
					reader = new StringReader(notebookXml);
					using (var xmlReader = new XmlTextReader(reader))
					{
						while (xmlReader.Read())
						{
							if (!"one:Section".Equals(xmlReader.Name))
							{
								continue;
							}
							string id = xmlReader.GetAttribute("ID");
							string path = xmlReader.GetAttribute("path");
							if (unfiledNotesPath.Equals(path))
							{
								return id;
							}
						}
					}
				}
				finally
				{
					if (reader != null)
					{
						reader.Dispose();
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Call this to get the running Excel application, returns null if there isn't any.
		/// </summary>
		/// <returns>ComDisposable for Excel.Application or null</returns>
		private static IDisposableCom<OneNote.Application> GetOneNoteApplication()
		{
			IDisposableCom<OneNote.Application> oneNoteApplication;
			try
			{
				oneNoteApplication = DisposableCom.Create((OneNote.Application) Marshal.GetActiveObject("OneNote.Application"));
			}
			catch
			{
				// Ignore, probably no OneNote running
				return null;
			}
			return oneNoteApplication;
		}

		/// <summary>
		/// Call this to get the running OneNote application, or create a new instance
		/// </summary>
		/// <returns>ComDisposable for OneNote.Application</returns>
		private static IDisposableCom<OneNote.Application> GetOrCreateOneNoteApplication()
		{
			IDisposableCom<OneNote.Application> oneNoteApplication = GetOneNoteApplication();
			if (oneNoteApplication == null)
			{
				oneNoteApplication = DisposableCom.Create(new OneNote.Application());
			}
			return oneNoteApplication;
		}

		/// <summary>
		/// Get the captions of all the open word documents
		/// </summary>
		/// <returns></returns>
		public static IList<OneNotePage> GetPages()
		{
			var pages = new List<OneNotePage>();
			try
			{
				using (var oneNoteApplication = GetOrCreateOneNoteApplication())
				{
					if (oneNoteApplication != null)
					{
						// ReSharper disable once RedundantAssignment
						string notebookXml = "";
						oneNoteApplication.ComObject.GetHierarchy("", OneNote.HierarchyScope.hsPages, out notebookXml, OneNote.XMLSchema.xs2010);
						if (!string.IsNullOrEmpty(notebookXml))
						{
							Log.Debug().WriteLine(notebookXml);
							StringReader reader = null;
							try
							{
								reader = new StringReader(notebookXml);
								using (var xmlReader = new XmlTextReader(reader))
								{
									reader = null;
									OneNoteSection currentSection = null;
									OneNoteNotebook currentNotebook = null;
									while (xmlReader.Read())
									{
										if ("one:Notebook".Equals(xmlReader.Name))
										{
											string id = xmlReader.GetAttribute("ID");
											if (id != null && (currentNotebook == null || !id.Equals(currentNotebook.Id)))
											{
												currentNotebook = new OneNoteNotebook();
												currentNotebook.Id = xmlReader.GetAttribute("ID");
												currentNotebook.Name = xmlReader.GetAttribute("name");
											}
										}
										if ("one:Section".Equals(xmlReader.Name))
										{
											string id = xmlReader.GetAttribute("ID");
											if (id != null && (currentSection == null || !id.Equals(currentSection.Id)))
											{
												currentSection = new OneNoteSection();
												currentSection.Id = xmlReader.GetAttribute("ID");
												currentSection.Name = xmlReader.GetAttribute("name");
												currentSection.Parent = currentNotebook;
											}
										}
										if ("one:Page".Equals(xmlReader.Name))
										{
											// Skip deleted items
											if ("true".Equals(xmlReader.GetAttribute("isInRecycleBin")))
											{
												continue;
											}
											var page = new OneNotePage();
											page.Parent = currentSection;
											page.Name = xmlReader.GetAttribute("name");
											page.Id = xmlReader.GetAttribute("ID");
											if (page.Id == null || page.Name == null)
											{
												continue;
											}
											page.IsCurrentlyViewed = "true".Equals(xmlReader.GetAttribute("isCurrentlyViewed"));
											pages.Add(page);
										}
									}
								}
							}
							finally
							{
								if (reader != null)
								{
									reader.Dispose();
								}
							}
						}
					}
				}
			}
			catch (COMException cEx)
			{
				if (cEx.ErrorCode == unchecked((int) 0x8002801D))
				{
					Log.Warn().WriteLine("Wrong registry keys, to solve this remove the OneNote key as described here: http://microsoftmercenary.com/wp/outlook-excel-interop-calls-breaking-solved/");
				}
				Log.Warn().WriteLine(cEx, "Problem retrieving onenote destinations, ignoring: ");
			}
			catch (Exception ex)
			{
				Log.Warn().WriteLine(ex, "Problem retrieving onenote destinations, ignoring: ");
			}
			pages.Sort((page1, page2) =>
			{
				if (page1.IsCurrentlyViewed || page2.IsCurrentlyViewed)
				{
					return page2.IsCurrentlyViewed.CompareTo(page1.IsCurrentlyViewed);
				}
				return string.Compare(page1.DisplayName, page2.DisplayName, StringComparison.Ordinal);
			});
			return pages;
		}
	}

	/// <summary>
	/// Container for transporting Page information
	/// </summary>
	public class OneNotePage
	{
		public OneNoteSection Parent
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}

		public bool IsCurrentlyViewed
		{
			get;
			set;
		}

		public string DisplayName
		{
			get
			{
				OneNoteNotebook notebook = Parent.Parent;
				if (string.IsNullOrEmpty(notebook.Name))
				{
					return string.Format("{0} / {1}", Parent.Name, Name);
				}
				return string.Format("{0} / {1} / {2}", Parent.Parent.Name, Parent.Name, Name);
			}
		}
	}

	/// <summary>
	/// Container for transporting section information
	/// </summary>
	public class OneNoteSection
	{
		public OneNoteNotebook Parent
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}
	}

	/// <summary>
	/// Container for transporting notebook information
	/// </summary>
	public class OneNoteNotebook
	{
		public string Name
		{
			get;
			set;
		}

		public string Id
		{
			get;
			set;
		}
	}
}