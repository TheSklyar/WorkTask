using Helpers.Settings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    public static class CommonSettingsReader
    {
        public static CommonSettings ReadFromSettings()
        {

            return new CommonSettings
            {
                AppVersion = Properties.Settings.Default.AppVersion,
                AppName = Properties.Settings.Default.AppName
            };
        }
    }
}
