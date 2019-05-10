using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Common
{
    internal class SqlConnectionStringFactory
    {
        public SqlConnectionStringFactory(ConnectionSettings connectionSettings)
        {
            _connectionSettings = Guard.GetNotNull(connectionSettings, "connectionSettings");

            Guard.CheckContainsText(
                _connectionSettings.ConnectionString,
                () => new ArgumentException("Connection string must contains a value."));
        }

        public SqlConnectionStringBuilder CreateConnectionString()
        {
            return new SqlConnectionStringBuilder(_connectionSettings.ConnectionString);
        }

        private readonly ConnectionSettings _connectionSettings;
    }
}
