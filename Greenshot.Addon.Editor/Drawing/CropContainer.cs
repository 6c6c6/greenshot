﻿//  Greenshot - a free and open source screenshot tool
//  Copyright (C) 2007-2017 Thomas Braun, Jens Klingen, Robin Krom
// 
//  For more information see: http://getgreenshot.org/
//  The Greenshot project is hosted on GitHub: https://github.com/greenshot
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 1 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

#region Usings

using System.Drawing;
using System.Runtime.Serialization;
using Greenshot.Addon.Extensions;
using Greenshot.Addon.Interfaces.Drawing;

#endregion

namespace Greenshot.Addon.Editor.Drawing
{
	/// <summary>
	///     Description of CropContainer.
	/// </summary>
	[Flag(ElementFlag.CONFIRMABLE)]
	public class CropContainer : DrawableContainer
	{
		public CropContainer(Surface parent) : base(parent)
		{
			Init();
		}

		/// <summary>
		///     We need to override the DrawingBound, return a rectangle in the size of the image, to make sure this element is
		///     always draw
		///     (we create a transparent brown over the complete picture)
		/// </summary>
		public override Rectangle DrawingBounds
		{
			get { return new Rectangle(0, 0, _parent.Width, _parent.Height); }
		}

		public override bool HasContextMenu
		{
			get
			{
				// No context menu for the CropContainer
				return false;
			}
		}

		public override void Draw(Graphics g, RenderMode rm)
		{
			if (_parent == null)
			{
				return;
			}
			using (Brush cropBrush = new SolidBrush(Color.FromArgb(100, 150, 150, 100)))
			{
				Rectangle cropRectangle = new Rectangle(Left, Top, Width, Height).MakeGuiRectangle();
				Rectangle selectionRect = new Rectangle(cropRectangle.Left - 1, cropRectangle.Top - 1, cropRectangle.Width + 1, cropRectangle.Height + 1);

				DrawSelectionBorder(g, selectionRect);

				// top
				g.FillRectangle(cropBrush, new Rectangle(0, 0, _parent.Width, cropRectangle.Top));
				// left
				g.FillRectangle(cropBrush, new Rectangle(0, cropRectangle.Top, cropRectangle.Left, cropRectangle.Height));
				// right
				g.FillRectangle(cropBrush, new Rectangle(cropRectangle.Left + cropRectangle.Width, cropRectangle.Top, _parent.Width - (cropRectangle.Left + cropRectangle.Width), cropRectangle.Height));
				// bottom
				g.FillRectangle(cropBrush, new Rectangle(0, cropRectangle.Top + cropRectangle.Height, _parent.Width, _parent.Height - (cropRectangle.Top + cropRectangle.Height)));
			}
		}

		private void Init()
		{
			CreateDefaultAdorners();
		}

		public override void Invalidate()
		{
			_parent?.Invalidate();
		}

		protected override void OnDeserialized(StreamingContext streamingContext)
		{
			base.OnDeserialized(streamingContext);
			Init();
		}
	}
}