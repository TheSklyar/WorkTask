
using Helpers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Settings
{
    public class PipesSettingsReader
    {
        public PipesSettingsReader(
            LauncherSettings launcherSettings,
            CommonSettings commonSettings)
        {
            _launcherSettings = launcherSettings;
            _commonSettings = commonSettings;
        }

        public void ReadFromCfg(ConnectionSettings connectionSettings)
        {
            var settings = new XmlSettings(_launcherSettings.PathToPipeSettings);
            connectionSettings.User = settings.GetValue(_commonSettings.AppName, "User", string.Empty);
            connectionSettings.ConfigFile = settings.GetValue(_commonSettings.AppName, "CfgFile", string.Empty);
            connectionSettings.Alias = settings.GetValue(_commonSettings.AppName, "Alias", string.Empty);
        }

        public void WriteToCfg(ConnectionSettings connectionSettings)
        {
            var settings = new XmlSettings(_launcherSettings.PathToPipeSettings);
            settings.SetValue(_commonSettings.AppName, "User", connectionSettings.User);
            settings.SetValue(_commonSettings.AppName, "CfgFile", connectionSettings.ConfigFile);
            settings.SetValue(_commonSettings.AppName, "Alias", connectionSettings.Alias);
        }

        private readonly CommonSettings _commonSettings;
        private readonly LauncherSettings _launcherSettings;
    }
}
