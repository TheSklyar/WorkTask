using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Helpers.Common;

namespace Payment.Data
{
    public class Money
    {
        public int ID { get; set; }
        public decimal Summ { get; set; }
        public decimal SummRest { get; set; }
        private ConnectionSettings _connectionSettings;
        public override string ToString()
        {
            return "Счет номер " + ID + " Сумма " + Summ + " Осталось " + SummRest+"\n";
        }
        public Money(ConnectionSettings connectionSettings, int id)
        {
            _connectionSettings = connectionSettings;
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetMoneyByID, _connection))
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
                        SummRest = reader.GetDecimal(1);
                    }
                }
                _connection.Close();
            }
        }
    }
}
