using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Helpers.Common;

namespace Payment.Data
{
    internal class PaymentsFilter : INotifyPropertyChanged
    {
        ConnectionSettings _connectionSettings;
        public Dictionary<int, string> _filters;
        public PaymentsFilter(ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _filters = new Dictionary<int, string>
            {
                {1, ""},
                {2, ""},
                {3, ""},
                {4, ""}
            };
        }

        public Dictionary<int, string> TableSourceForSort = new Dictionary<int, string>
        {
            {1,@"tor.ID" },
            {2,@"tor.[Summ]" }
        };

        private int _NumberFilterText;
        public string NumberFilterText
        {
            get => _NumberFilterText == 0 ? "" : _NumberFilterText.ToString();
            set
            {
                _NumberFilterText = Convert.ToInt32(string.IsNullOrWhiteSpace(value) ? "0" : value);
                NumberFilterTextForm();
                OnPropertyChanged();
            }
        }
        public void NumberFilterTextForm()
        {
            if (!string.IsNullOrWhiteSpace(NumberFilterText))
            {
                _filters[1] = "tor.[ID] = " + _NumberFilterText + "";
            }
            else
            {
                _filters[1] = "";
            }
        }

        private decimal _SummFilterText;
        public string SummFilterText
        {
            get => _SummFilterText == 0 ? "" : _SummFilterText.ToString();
            set
            {
                _SummFilterText = Convert.ToDecimal(Extensions.PrepareStringToConvert(string.IsNullOrEmpty(value) ? "0" : value));
                SummFilterTextForm();
                OnPropertyChanged();
            }
        }
        public void SummFilterTextForm()
        {
            if (_SummFilterText != 0)
            {
                _filters[2] = "tor.[Summ] = " + _SummFilterText.ToString().Replace(',','.') + "";
            }
            else
            {
                _filters[2] = "";
            }
        }

        private int _OrderNumFilterText;
        public string OrderNumFilterText
        {
            get => _OrderNumFilterText == 0 ? "" : _OrderNumFilterText.ToString();
            set
            {
                _OrderNumFilterText = Convert.ToInt32(string.IsNullOrWhiteSpace(value) ? "0" : value);
                if (_OrderNumFilterText==0)
                {
                    _filters[3] = "";
                }
                else
                {
                    _filters[3] = "(tor.ID in (" + GetIDsByOrder(_OrderNumFilterText) + "))";
                }
                OnPropertyChanged();
            }
        }

        private int _MoneyNumFilterText;
        public string MoneyNumFilterText
        {
            get => _MoneyNumFilterText == 0 ? "" : _MoneyNumFilterText.ToString();
            set
            {
                _MoneyNumFilterText = Convert.ToInt32(string.IsNullOrWhiteSpace(value) ? "0" : value);
                if (_MoneyNumFilterText == 0)
                {
                    _filters[4] = "";
                }
                else
                {
                    _filters[4] = "(tor.ID in (" + GetIDsByMoney(_MoneyNumFilterText) + "))";
                }
                OnPropertyChanged();
            }
        }

        private object GetIDsByOrder(int ID)
        {
            List<int> ids = new List<int>();
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetAllIDsByOrder, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(0));
                    }
                }
                _connection.Close();
            }
            string result = "";
            foreach (var item in ids)
            {
                result += item + ",";
            }
            result = result.Remove(result.Length - 1);
            return result;
        }

        private object GetIDsByMoney(int ID)
        {
            List<int> ids = new List<int>();
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetAllIDsByMoney, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", ID);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt32(0));
                    }
                }
                _connection.Close();
            }
            string result = "";
            foreach (var item in ids)
            {
                result += item + ",";
            }
            result = result.Remove(result.Length - 1);
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
