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
using System.Drawing.Drawing2D;

#endregion

namespace Greenshot.Core.Gfx
{
	/// <summary>
	///     ResizeEffect
	/// </summary>
	public class ResizeEffect : IEffect
	{
		public ResizeEffect(int width, int height, bool maintainAspectRatio)
		{
			Width = width;
			Height = height;
			MaintainAspectRatio = maintainAspectRatio;
		}

		public int Height { get; set; }

		public bool MaintainAspectRatio { get; set; }

		public int Width { get; set; }

		public void Reset()
		{
			// values don't have a default value
		}

		public string Name { get; } = "editor_resize";

		public Image Apply(Image sourceImage, Matrix matrix)
		{
			return ImageHelper.ResizeImage(sourceImage, MaintainAspectRatio, Width, Height, matrix);
		}
	}
}