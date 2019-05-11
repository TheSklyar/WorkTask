using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Helpers.Common;
using Helpers.Enums;
using Money;
using Order;
using Payment.Data;

namespace Payment
{
    /// <summary>
    /// Логика взаимодействия для PaymentEdit.xaml
    /// </summary>
    public partial class PaymentEdit : Window
    {
        private ConnectionSettings _connectionSettings;
        private OpenType type;
        public PaymentEdit(ConnectionSettings connectionSettings, OpenType type, int id = 0)
        {
            _connectionSettings = connectionSettings;
            this.type = type;
            InitializeComponent();
            if (type == OpenType.View)
            {
                AddButton.Visibility = Visibility.Hidden;
                IDField.Text = id.ToString();
            }
            else
            {
                AddButton.Visibility = Visibility.Visible;
                if (id != 0)
                {
                    IDField.Text = id.ToString();
                }
            }
            if (type != OpenType.New)
            {
                FillData(id);
            }
            else
            {
                SummField.Text = "0";
            }
        }

        private void FillData(int id)
        {
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetAllDataByID, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", UserID);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AddDepartment(reader.IsDBNull(0) ? "" : reader.GetString(0));
                    }
                }
                _connection.Close();
            }
        }

        private void AddData(int mID, int oID, decimal summ)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (tempMonID == 0 || tempOrdID == 0 || Summ.Text=="")
            {
                MessageBox.Show("Заказ, дееньги и сумма должны быть указаны!");
                return;
            }
            var tempPay = new Pay(_connectionSettings, tempMonID, tempOrdID, Convert.ToDecimal());
            if (tempPay.o is null || tempPay.m is null)
            {
                return;
            }

            if (Pays.ContainsKey(tempPay.ToString()))
            {
                MessageBox.Show("Такой элемент уже есть в списке!");
                return;
            }
            Pays.Add(tempPay.ToString(), tempPay);
            PayPanels.Add(tempPay.ToString(), tempPay.Create());
            tempPay.Button.Click += (o, args) =>
            {
                PayList.Items.Remove(PayPanels[tempPay.ToString()]);
                PayPanels.Remove(tempPay.ToString());
                Pays.Remove(tempPay.ToString());
            };
            tempPay.Button.Visibility = type == OpenType.View ? Visibility.Collapsed : Visibility.Visible;
            PayList.Items.Add(PayPanels[tempPay.ToString()]);
        }
        public Dictionary<string, Pay> Pays = new Dictionary<string, Pay>();
        public Dictionary<string, DockPanel> PayPanels = new Dictionary<string, DockPanel>();

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox dgc = sender as TextBox;
            var tt = new ToolTip();
            var toolTipPanel = new StackPanel();
            toolTipPanel.Children.Add(new TextBlock { Text = "Сумма должна быть меньше либо равна остатку выплат по заказу и остатка выбранных денег", FontSize = 10, MaxWidth = 500, TextWrapping = TextWrapping.Wrap });
            tt.Content = toolTipPanel;
            dgc.ToolTip = tt;
        }

        private int tempMonID=0, tempOrdID=0;
        private void Ord_click(object sender, RoutedEventArgs e)
        {
            var cds = new OrdersWindow(_connectionSettings, true);
            cds.Owner = this;
            cds.ShowDialog();
            if (TemporaryStorage.Holder.TryGetValue("ID", out string ID))
            {
                tempOrdID = Convert.ToInt32(ID);
                OrderID.Text = tempOrdID.ToString();
            }
            TemporaryStorage.Holder.Remove("ID");
        }

        private void Mon_click(object sender, RoutedEventArgs e)
        {
            var cds = new MoneyWindow(_connectionSettings, true);
            cds.Owner = this;
            cds.ShowDialog();
            if (TemporaryStorage.Holder.TryGetValue("ID", out string ID))
            {
                tempMonID = Convert.ToInt32(ID);
                MoneyID.Text = tempMonID.ToString();
            }
            TemporaryStorage.Holder.Remove("ID");
        }
    }
}
