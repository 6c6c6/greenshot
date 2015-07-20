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
using System.Reflection;

// Remove AppendPrivatePath warning:
using Greenshot.Forms;

#pragma warning disable 0618
namespace Greenshot {
	/// <summary>
	/// Description of Main.
	/// </summary>
	public class GreenshotMain {
		static GreenshotMain() {
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		}

		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			Assembly ayResult = null;
			string _shortAssemblyName = args.Name.Split(',')[0];
			Assembly[] ayAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly ayAssembly in ayAssemblies) {
				if (_shortAssemblyName == ayAssembly.FullName.Split(',')[0]) {
					ayResult = ayAssembly;
					break;
				}
			}
			return ayResult;
		}

		[STAThread]
		public static void Main(string[] args) {
			MainForm.Start(args);
		}
	}
}
