/* 
    Gekko Timeseries Software (www.t-t.dk/gekko).
    Copyright (C) 2021, Thomas Thomsen, T-T Analyse.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program (see the file COPYING in the root folder).
    Else, see <http://www.gnu.org/licenses/>.        
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;


namespace Gekko
{
    //This class stores usersettings between sessions, without having to deal with where precisely the settings
    //are stored (usually in a .xml file somewhere).
    //So using ApplicationSettingsBase as a base is very convenient.
    public class UserSettings : ApplicationSettingsBase
    {        
        [DefaultSettingValue("true")]
        [UserScopedSetting()]
        public bool UpgradeRequired
        {
            get
            {
                return ((bool)this["UpgradeRequired"]);
            }
            set
            {
                this["UpgradeRequired"] = (bool)value;
            }
        }
        [DefaultSettingValue("700")]
        [UserScopedSetting()]        
        public int MainWindowWidth
        {
            get
            {
                return ((int)this["MainWindowWidth"]);
            }
            set
            {
                this["MainWindowWidth"] = (int)value;
            }
        }
        [DefaultSettingValue("50")]
        [UserScopedSetting()]
        public int MainWindowTopDistance
        {
            get
            {
                return ((int)this["MainWindowTopDistance"]);
            }
            set
            {
                this["MainWindowTopDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("100")]
        [UserScopedSetting()]
        public int MainWindowLeftDistance
        {
            get
            {
                return ((int)this["MainWindowLeftDistance"]);
            }
            set
            {
                this["MainWindowLeftDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("276")]
        [UserScopedSetting()]
        public int MainWindowSplitterDistance
        {
            get
            {
                return ((int)this["MainWindowSplitterDistance"]);
            }
            set
            {
                this["MainWindowSplitterDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("550")]
        [UserScopedSetting()]
        public int MainWindowHeight
        {
            get
            {
                return ((int)this["MainWindowHeight"]);
            }
            set
            {
                this["MainWindowHeight"] = (int)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]        
        public String TspUtilityPath
        {
            get
            {
                return ((String)this["TspUtilityPath"]);
            }
            set
            {
                this["TspUtilityPath"] = (String)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public String WorkingFolder
        {
            get
            {
                return ((String)this["WorkingFolder"]);
            }
            set
            {
                this["WorkingFolder"] = (String)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders1
        {
            get
            {
                return ((string)this["RecentFolders1"]);
            }
            set
            {
                this["RecentFolders1"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders2
        {
            get
            {
                return ((string)this["RecentFolders2"]);
            }
            set
            {
                this["RecentFolders2"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders3
        {
            get
            {
                return ((string)this["RecentFolders3"]);
            }
            set
            {
                this["RecentFolders3"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders4
        {
            get
            {
                return ((string)this["RecentFolders4"]);
            }
            set
            {
                this["RecentFolders4"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders5
        {
            get
            {
                return ((string)this["RecentFolders5"]);
            }
            set
            {
                this["RecentFolders5"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders6
        {
            get
            {
                return ((string)this["RecentFolders6"]);
            }
            set
            {
                this["RecentFolders6"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders7
        {
            get
            {
                return ((string)this["RecentFolders7"]);
            }
            set
            {
                this["RecentFolders7"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders8
        {
            get
            {
                return ((string)this["RecentFolders8"]);
            }
            set
            {
                this["RecentFolders8"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders9
        {
            get
            {
                return ((string)this["RecentFolders9"]);
            }
            set
            {
                this["RecentFolders9"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders10
        {
            get
            {
                return ((string)this["RecentFolders10"]);
            }
            set
            {
                this["RecentFolders10"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders11
        {
            get
            {
                return ((string)this["RecentFolders11"]);
            }
            set
            {
                this["RecentFolders11"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders12
        {
            get
            {
                return ((string)this["RecentFolders12"]);
            }
            set
            {
                this["RecentFolders12"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders13
        {
            get
            {
                return ((string)this["RecentFolders13"]);
            }
            set
            {
                this["RecentFolders13"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders14
        {
            get
            {
                return ((string)this["RecentFolders14"]);
            }
            set
            {
                this["RecentFolders14"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders15
        {
            get
            {
                return ((string)this["RecentFolders15"]);
            }
            set
            {
                this["RecentFolders15"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders16
        {
            get
            {
                return ((string)this["RecentFolders16"]);
            }
            set
            {
                this["RecentFolders16"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders17
        {
            get
            {
                return ((string)this["RecentFolders17"]);
            }
            set
            {
                this["RecentFolders17"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders18
        {
            get
            {
                return ((string)this["RecentFolders18"]);
            }
            set
            {
                this["RecentFolders18"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders19
        {
            get
            {
                return ((string)this["RecentFolders19"]);
            }
            set
            {
                this["RecentFolders19"] = (string)value;
            }
        }
        [DefaultSettingValue("")]
        [UserScopedSetting()]
        public string RecentFolders20
        {
            get
            {
                return ((string)this["RecentFolders20"]);
            }
            set
            {
                this["RecentFolders20"] = (string)value;
            }
        }        
        [DefaultSettingValue("50")]
        [UserScopedSetting()]
        public int GraphWindowTopDistance
        {
            get
            {
                return ((int)this["GraphWindowTopDistance"]);
            }
            set
            {
                this["GraphWindowTopDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("100")]
        [UserScopedSetting()]
        public int GraphWindowLeftDistance
        {
            get
            {
                return ((int)this["GraphWindowLeftDistance"]);
            }
            set
            {
                this["GraphWindowLeftDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("50")]
        [UserScopedSetting()]
        public int DecompWindowTopDistance
        {
            get
            {
                return ((int)this["DecompWindowTopDistance"]);
            }
            set
            {
                this["DecompWindowTopDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("100")]
        [UserScopedSetting()]
        public int DecompWindowLeftDistance
        {
            get
            {
                return ((int)this["DecompWindowLeftDistance"]);
            }
            set
            {
                this["DecompWindowLeftDistance"] = (int)value;
            }
        }

        [DefaultSettingValue("600")]
        [UserScopedSetting()]
        public int DecompWindowHeightDistance
        {
            get
            {
                return ((int)this["DecompWindowHeightDistance"]);
            }
            set
            {
                this["DecompWindowHeightDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("900")]
        [UserScopedSetting()]
        public int DecompWindowWidthDistance
        {
            get
            {
                return ((int)this["DecompWindowWidthDistance"]);
            }
            set
            {
                this["DecompWindowWidthDistance"] = (int)value;
            }
        }

        [DefaultSettingValue("700")]
        [UserScopedSetting()]
        public int DecompWindowSplitterHorizontal
        {
            get
            {
                return ((int)this["DecompWindowSplitterHorizontal"]);
            }
            set
            {
                this["DecompWindowSplitterHorizontal"] = (int)value;
            }
        }
        [DefaultSettingValue("250")]
        [UserScopedSetting()]
        public int DecompWindowSplitterVertical
        {
            get
            {
                return ((int)this["DecompWindowSplitterVertical"]);
            }
            set
            {
                this["DecompWindowSplitterVertical"] = (int)value;
            }
        }
        
        [DefaultSettingValue("100")]
        [UserScopedSetting()]
        public int ErrorWindowTopDistance
        {
            get
            {
                return ((int)this["ErrorWindowTopDistance"]);
            }
            set
            {
                this["ErrorWindowTopDistance"] = (int)value;
            }
        }
        [DefaultSettingValue("150")]
        [UserScopedSetting()]
        public int ErrorWindowLeftDistance
        {
            get
            {
                return ((int)this["ErrorWindowLeftDistance"]);
            }
            set
            {
                this["ErrorWindowLeftDistance"] = (int)value;
            }
        }
    }
}

