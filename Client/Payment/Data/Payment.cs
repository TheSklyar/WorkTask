using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Helpers.Common;

namespace Payment.Data
{
    public class Pay
    {
        public Money m;
        public Order o;
        public decimal SummPay { get; set; }
        public Button Button;
        public override string ToString()
        {
            return o.ToString() + m.ToString()+"Сумма: " +SummPay;
        }

        private ConnectionSettings _connectionSettings;
        private int mID, oID;
        public Pay(ConnectionSettings connectionSettings, int monID, int ordID, decimal summ)
        {
            mID = monID;
            oID = ordID;
            SummPay = summ;
            m = new Money(connectionSettings, monID);
            o = new Order(connectionSettings, ordID);
            if (m.Summ==0 || o.Summ==0)
            {
                MessageBox.Show("Попытка добавить деньги или заказ с нулевой суммой!");
                m = null;
                o = null;
            }

            _connectionSettings = connectionSettings;
        }
        public DockPanel Create()
        {
            Button = new Button
            {
                FontSize = 12,
                Margin = new Thickness(0, 0, 5, 0),
                Content = "Удалить"
            };
            var TextBlock = new TextBlock
            {
                FontSize = 12,
                Text = this.ToString(),
                VerticalAlignment = VerticalAlignment.Center
            };
            var DockPanel = new DockPanel { Margin = new Thickness(2)};
            DockPanel.Children.Add(Button);
            DockPanel.Children.Add(TextBlock);
            return DockPanel;
        }

        public bool Save(int id)
        {
            int res = 0;
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.SaveItem, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", id);
                }
                if (!command.Parameters.Contains("@MID"))
                {
                    command.Parameters.AddWithValue("@MID", m.ID);
                }
                if (!command.Parameters.Contains("@OID"))
                {
                    command.Parameters.AddWithValue("@OID", o.ID);
                }
                if (!command.Parameters.Contains("@Summ"))
                {
                    command.Parameters.AddWithValue("@Summ", SummPay);
                }
                _connection.Open();
                res = command.ExecuteNonQuery();
                _connection.Close();
            }

            if (0 == res)
            {
                MessageBox.Show("Внимание! Произошла ошибка при сохранении!");
                return false;
            }

            return true;
        }

        public bool Delete(int id)
        {
            int res = 0;
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.DelItem, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", id);
                }
                if (!command.Parameters.Contains("@MID"))
                {
                    command.Parameters.AddWithValue("@MID", m.ID);
                }
                if (!command.Parameters.Contains("@OID"))
                {
                    command.Parameters.AddWithValue("@OID", o.ID);
                }
                if (!command.Parameters.Contains("@Summ"))
                {
                    command.Parameters.AddWithValue("@Summ", SummPay);
                }
                _connection.Open();
                res = command.ExecuteNonQuery();
                _connection.Close();
            }

            if (0 == res)
            {
                MessageBox.Show("Внимание! Произошла ошибка при удалении!");
                return false;
            }

            return true;
        }
    }
}
