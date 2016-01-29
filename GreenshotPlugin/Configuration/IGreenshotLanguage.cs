/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2016 Thomas Braun, Jens Klingen, Robin Krom
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

using System.ComponentModel;
using Dapplo.Config.Language;

namespace GreenshotPlugin.Configuration
{
	[Language("Core")]
	public interface IGreenshotLanguage : ILanguage, INotifyPropertyChanged, IEditorLanguage
	{
		string AboutBugs
		{
			get;
		}

		string AboutDonations
		{
			get;
		}

		string AboutHost
		{
			get;
		}

		string AboutIcons
		{
			get;
		}

		string AboutLicense
		{
			get;
		}

		string AboutTitle
		{
			get;
		}

		string AboutTranslation
		{
			get;
		}

		string ApplicationTitle
		{
			get;
		}

		string BugreportCancel
		{
			get;
		}

		string BugreportInfo
		{
			get;
		}

		string BugreportTitle
		{
			get;
		}

		string ClipboardError
		{
			get;
		}

		string ClipboardInuse
		{
			get;
		}

		string ColorpickerAlpha
		{
			get;
		}

		string ColorpickerApply
		{
			get;
		}

		string ColorpickerBlue
		{
			get;
		}

		string ColorpickerGreen
		{
			get;
		}

		string ColorpickerHtmlcolor
		{
			get;
		}

		string ColorpickerRecentcolors
		{
			get;
		}

		string ColorpickerRed
		{
			get;
		}

		string ColorpickerTitle
		{
			get;
		}

		string ColorpickerTransparent
		{
			get;
		}

		string ConfigUnauthorizedaccessWrite
		{
			get;
		}

		string ContextmenuAbout
		{
			get;
		}

		string ContextmenuCapturearea
		{
			get;
		}

		string ContextmenuCaptureclipboard
		{
			get;
		}

		string ContextmenuCapturefullscreen
		{
			get;
		}

		string ContextmenuCapturefullscreenAll
		{
			get;
		}

		string ContextmenuCapturefullscreenLeft
		{
			get;
		}

		string ContextmenuCapturefullscreenTop
		{
			get;
		}

		string ContextmenuCapturefullscreenRight
		{
			get;
		}

		string ContextmenuCapturefullscreenBottom
		{
			get;
		}

		string ContextmenuCapturelastregion
		{
			get;
		}

		string ContextmenuCapturewindow
		{
			get;
		}

		string ContextmenuDonate
		{
			get;
		}

		string ContextmenuExit
		{
			get;
		}

		string ContextmenuHelp
		{
			get;
		}

		string ContextmenuOpenfile
		{
			get;
		}

		string ContextmenuQuicksettings
		{
			get;
		}

		string ContextmenuSettings
		{
			get;
		}

		string ContextmenuCaptureie
		{
			get;
		}

		string ContextmenuOpenrecentcapture
		{
			get;
		}

		[DefaultValue("Error")]
        string Error
		{
			get;
		}

		[DefaultValue("An instance of Greenshot is already running.")]
		string ErrorMultipleinstances
		{
			get;
		}

		string ErrorNowriteaccess
		{
			get;
		}

		string ErrorOpenfile
		{
			get;
		}

		[DefaultValue("Could not open link '{0}'.")]
		string ErrorOpenlink
		{
			get;
		}

		string ErrorSave
		{
			get;
		}

		string ErrorSaveInvalidChars
		{
			get;
		}

		string HelpTitle
		{
			get;
		}

		string JpegqualitydialogChoosejpegquality
		{
			get;
		}

		string QualitydialogDontaskagain
		{
			get;
		}

		string QualitydialogTitle
		{
			get;
		}

		string SettingsReducecolors
		{
			get;
		}

		string PrintError
		{
			get;
		}

		string PrintoptionsAllowcenter
		{
			get;
		}

		string PrintoptionsAllowenlarge
		{
			get;
		}

		string PrintoptionsAllowrotate
		{
			get;
		}

		string PrintoptionsAllowshrink
		{
			get;
		}

		string PrintoptionsColors
		{
			get;
		}

		string PrintoptionsDontaskagain
		{
			get;
		}

		string PrintoptionsPagelayout
		{
			get;
		}

		string PrintoptionsPrintcolor
		{
			get;
		}

		string PrintoptionsPrintgrayscale
		{
			get;
		}

		string PrintoptionsPrintmonochrome
		{
			get;
		}

		string PrintoptionsTimestamp
		{
			get;
		}

		string PrintoptionsInverted
		{
			get;
		}

		string PrintoptionsTitle
		{
			get;
		}

		string QuicksettingsDestinationFile
		{
			get;
		}

		string SettingsAlwaysshowqualitydialog
		{
			get;
		}

		string SettingsAlwaysshowprintoptionsdialog
		{
			get;
		}

		string SettingsApplicationsettings
		{
			get;
		}

		string SettingsAutostartshortcut
		{
			get;
		}

		string SettingsCapture
		{
			get;
		}

		string SettingsCaptureMousepointer
		{
			get;
		}

		string SettingsCaptureWindowsInteractive
		{
			get;
		}

		string SettingsCopypathtoclipboard
		{
			get;
		}

		string SettingsDestination
		{
			get;
		}

		string SettingsDestinationClipboard
		{
			get;
		}

		string SettingsDestinationEmail
		{
			get;
		}

		string SettingsDestinationFile
		{
			get;
		}

		string SettingsDestinationFileas
		{
			get;
		}

		string SettingsDestinationPrinter
		{
			get;
		}

		string SettingsDestinationPicker
		{
			get;
		}

		string SettingsEditor
		{
			get;
		}

		string SettingsFilenamepattern
		{
			get;
		}

		string SettingsGeneral
		{
			get;
		}

		string SettingsIecapture
		{
			get;
		}

		string SettingsJpegquality
		{
			get;
		}

		string SettingsQualitysettings
		{
			get;
		}

		string SettingsLanguage
		{
			get;
		}

		string SettingsMessageFilenamepattern
		{
			get;
		}

		string SettingsOutput
		{
			get;
		}

		string SettingsPlaysound
		{
			get;
		}

		string SettingsPlugins
		{
			get;
		}

		string SettingsPluginsName
		{
			get;
		}

		string SettingsPluginsVersion
		{
			get;
		}

		string SettingsPluginsCreatedby
		{
			get;
		}

		string SettingsPluginsDllpath
		{
			get;
		}

		string SettingsPreferredfilesettings
		{
			get;
		}

		string SettingsPrimaryimageformat
		{
			get;
		}

		string SettingsPrinter
		{
			get;
		}

		string SettingsPrintoptions
		{
			get;
		}

		string SettingsRegisterhotkeys
		{
			get;
		}

		string SettingsShowflashlight
		{
			get;
		}

		string SettingsStoragelocation
		{
			get;
		}

		string SettingsTitle
		{
			get;
		}

		string SettingsTooltipFilenamepattern
		{
			get;
		}

		string SettingsTooltipLanguage
		{
			get;
		}

		string SettingsTooltipPrimaryimageformat
		{
			get;
		}

		string SettingsTooltipRegisterhotkeys
		{
			get;
		}

		string SettingsTooltipStoragelocation
		{
			get;
		}

		string SettingsVisualization
		{
			get;
		}

		string SettingsShownotify
		{
			get;
		}

		string SettingsWaittime
		{
			get;
		}

		string SettingsWindowscapture
		{
			get;
		}

		string SettingsWindowCaptureMode
		{
			get;
		}

		string SettingsNetwork
		{
			get;
		}

		string SettingsCheckperiod
		{
			get;
		}

		string SettingsUsedefaultproxy
		{
			get;
		}

		string TooltipFirststart
		{
			get;
		}

		string Warning
		{
			get;
		}

		string WarningHotkeys
		{
			get;
		}

		string Hotkeys
		{
			get;
		}

		string WaitIeCapture
		{
			get;
		}

		string UpdateFound
		{
			get;
		}

		string ExportedTo
		{
			get;
		}

		string DestinationExportFailed
		{
			get;
		}
	}
}