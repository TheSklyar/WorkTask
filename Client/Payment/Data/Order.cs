using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Common;

namespace Payment.Data
{
    public class Order
    {
        public int ID { get; set; }
        public decimal Summ { get; set; }
        public decimal SummPayed { get; set; }
        private ConnectionSettings _connectionSettings;
        public Order(ConnectionSettings connectionSettings, int id)
        {
            _connectionSettings = connectionSettings;
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetOrderByID, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", id);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ID = id;
                        Summ = reader.GetDecimal(0);
                        SummPayed = reader.GetDecimal(1);
                    }
                }
                _connection.Close();
            }
        }
    }
}
