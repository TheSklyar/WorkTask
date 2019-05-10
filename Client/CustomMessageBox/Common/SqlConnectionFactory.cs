using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Helpers.Common
{
    internal class SqlConnectionFactory
    {
        public SqlConnectionFactory(ConnectionSettings connectionSettings)
        {
            _connectionSettings = Guard.GetNotNull(connectionSettings, "connectionSettings");

            Guard.CheckContainsText(
                _connectionSettings.ConnectionString,
                () => new ArgumentException("Connection string must contains a value."));
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionSettings.ConnectionString);
        }

        private readonly ConnectionSettings _connectionSettings;
    }
}
