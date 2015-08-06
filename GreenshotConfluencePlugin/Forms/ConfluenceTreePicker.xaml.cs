﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2015 Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
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
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace GreenshotConfluencePlugin {
	/// <summary>
	/// Interaction logic for ConfluenceTreePicker.xaml
	/// </summary>
	public partial class ConfluenceTreePicker : System.Windows.Controls.Page {
		private static readonly log4net.ILog LOG = log4net.LogManager.GetLogger(typeof(ConfluenceTreePicker));
		private ConfluenceUpload _confluenceUpload;

		public ConfluenceTreePicker(ConfluenceUpload confluenceUpload) {
			_confluenceUpload = confluenceUpload;
			InitializeComponent();
		}

		async void pageTreeViewItem_DoubleClick(object sender, MouseButtonEventArgs eventArgs) {
			LOG.Debug("spaceTreeViewItem_MouseLeftButtonDown is called!");
			TreeViewItem clickedItem = eventArgs.Source as TreeViewItem;
			if (clickedItem ==null) {
				return;
			}
			var page = clickedItem.Tag as PageDetails;
			if (page == null) {
				return;
			}
			if (!clickedItem.HasItems) {
				LOG.Debug("Loading pages for page: " + page.Title);
				var pages = await ConfluencePlugin.ConfluenceAPI.GetChildrenAsync(page.Id);
				ShowBusy.Visibility = Visibility.Visible;
				foreach (var childPage in pages) {
					LOG.Debug("Adding page: " + childPage.Title);
					TreeViewItem pageTreeViewItem = new TreeViewItem();
					pageTreeViewItem.Header = childPage.Title;
					pageTreeViewItem.Tag = childPage;
					clickedItem.Items.Add(pageTreeViewItem);
					pageTreeViewItem.PreviewMouseDoubleClick += new MouseButtonEventHandler(pageTreeViewItem_DoubleClick);
					pageTreeViewItem.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(pageTreeViewItem_Click);
				}
				ShowBusy.Visibility = Visibility.Collapsed;
			}
		}
		
		void pageTreeViewItem_Click(object sender, MouseButtonEventArgs eventArgs) {
			LOG.Debug("pageTreeViewItem_PreviewMouseDoubleClick is called!");
			var clickedItem = eventArgs.Source as TreeViewItem;
			if (clickedItem ==null) {
				return;
			}
			var page = clickedItem.Tag as PageDetails;
			_confluenceUpload.SelectedPage = page;
			if (page != null) {
				LOG.Debug("Page selected: " + page.Title);
			}
		}

		async void Page_Loaded(object sender, RoutedEventArgs e) {
			_confluenceUpload.SelectedPage = null;
			ShowBusy.Visibility = Visibility.Visible;
			foreach (var space in _confluenceUpload.Spaces) {
				var spaceTreeViewItem = new TreeViewItem();
				spaceTreeViewItem.Header = space.Name;
				spaceTreeViewItem.Tag = space;

				// Get homepage
				try {
					var page = await ConfluencePlugin.ConfluenceAPI.GetChildrenAsync(space);
					var pageTreeViewItem = new TreeViewItem();
					pageTreeViewItem.Header = page.Title;
					pageTreeViewItem.Tag = page;
					pageTreeViewItem.PreviewMouseDoubleClick += pageTreeViewItem_DoubleClick;
					pageTreeViewItem.PreviewMouseLeftButtonDown += pageTreeViewItem_Click;
					spaceTreeViewItem.Items.Add(pageTreeViewItem);
					ConfluenceTreeView.Items.Add(spaceTreeViewItem);
				} catch (Exception ex) {
					LOG.Error("Can't get homepage for space : " + space.Name + " (" + ex.Message + ")");
				}
			}
			ShowBusy.Visibility = Visibility.Collapsed;
		}
	}
}