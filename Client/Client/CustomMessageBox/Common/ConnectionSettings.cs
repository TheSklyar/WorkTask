using Helpers.Settings;
using Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    public class ConnectionSettings
    {
        public ConnectionSettings(PipesSettingsReader pipesSettingsReader, LauncherSettings launcherSettings)
        {
            _pipesSettingsReader = Guard.GetNotNull(pipesSettingsReader, "pipesSettingsReader");
            Guard.CheckNotNull(launcherSettings, "launcherSettings");
            ReadFromCfg();
            ConnectionString = launcherSettings.DefaultConnectionString;
        }

        public string Alias { get; set; }

        public string ConfigFile { get; set; }

        public string ConnectionString { get; set; }

        public string DataBase
        {
            get
            {
                return string.IsNullOrWhiteSpace(ConnectionString)
                    ? string.Empty
                    : new SqlConnectionStringBuilder(ConnectionString).InitialCatalog;
            }
        }

        public string Password { get; set; }

        public string Server
        {
            get
            {
                return string.IsNullOrWhiteSpace(ConnectionString)
                    ? string.Empty
                    : new SqlConnectionStringBuilder(ConnectionString).DataSource;
            }
        }

        public string User { get; set; }

        public void ReadFromCfg()
        {
            _pipesSettingsReader.ReadFromCfg(this);
        }

        public void WriteToCfg()
        {
            _pipesSettingsReader.WriteToCfg(this);
        }

        private readonly PipesSettingsReader _pipesSettingsReader;
    }
}
