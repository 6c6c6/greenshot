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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;

namespace GreenshotWindowsSharePlugin.Native
{
	public class DataTransferManagerHelper
	{
		private const string DataTransferManagerId = "a5caee9b-8708-49d1-8d36-67d25a8da00c";
		private readonly IDataTransferManagerInterOp _interop;
		private readonly IntPtr _windowHandle;

		public DataTransferManager DataTransferManager
		{
			get;
			private set;
		}

		public DataTransferManagerHelper(IntPtr handle)
		{
			//TODO: Add a check for failure here. This will fail for versions of Windows below Windows 10
			IActivationFactory factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(DataTransferManager));

			// ReSharper disable once SuspiciousTypeConversion.Global
			_interop = (IDataTransferManagerInterOp)factory;

			_windowHandle = handle;
			DataTransferManager dtm;
			var riid = new Guid(DataTransferManagerId);
			_interop.GetForWindow(_windowHandle, riid, out dtm);

			DataTransferManager = dtm;
		}

		public void ShowShareUi()
		{
			_interop.ShowShareUIForWindow(_windowHandle);
		}
	}

}
