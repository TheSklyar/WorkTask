using Helpers.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    internal static class LauncherSettingsReader
    {
        public static LauncherSettings ReadFromSettings()
        {
            return new LauncherSettings
            {
                PathToAppSettings = Properties.Settings.Default.PathToAppSettings,
                PathToPipeSettings = Properties.Settings.Default.PathToPipeSettings,
                DefaultConnectionString = Properties.Settings.Default.DefaultConnectionString
            };
        }
    }
}
