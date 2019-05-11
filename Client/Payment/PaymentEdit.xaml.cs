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
        private int Id;
        public PaymentEdit(ConnectionSettings connectionSettings, OpenType type, int id = 0)
        {
            _connectionSettings = connectionSettings;
            this.type = type;
            InitializeComponent();
            if (type == OpenType.View)
            {
                DockBottom.Visibility = Visibility.Hidden;
                IDField.Text = id.ToString();
            }
            else
            {
                DockBottom.Visibility = Visibility.Visible;
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
                var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                using (var command = new SqlCommand(SqlCommands.SaveNew, _connection))
                {
                    _connection.Open();
                    id = Convert.ToInt32(command.ExecuteScalar());
                    _connection.Close();
                }

                if (id != 0)
                {
                    FillData(id);
                }
                
            }

            Id = id;
            IDField.Text = Id.ToString();
        }

        private void Reload()
        {
            type = OpenType.Edit;
            PayList.Items.Clear();
            PayPanels = new Dictionary<string, DockPanel>();
            Pays = new Dictionary<string, Pay>();
            SummField.Text = "0";
            FillData(Id);
        }

        private void FillData(int id)
        {
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.GetAllDataByID, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", id);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AddData(reader.GetInt32(0), reader.GetInt32(1), reader.GetDecimal(2));
                    }
                }
                _connection.Close();
            }
        }

        private void AddData(int mID, int oID, decimal summ)
        {
            var tempPay = new Pay(_connectionSettings, mID, oID, summ);
            Pays.Add(tempPay.ToString(), tempPay);
            PayPanels.Add(tempPay.ToString(), tempPay.Create());
            tempPay.Button.Click += (o, args) =>
            {
                if (tempPay.Delete(Id))
                {
                    PayList.Items.Remove(PayPanels[tempPay.ToString()]);
                    PayPanels.Remove(tempPay.ToString());
                    Pays.Remove(tempPay.ToString());
                }
            };
            tempPay.Button.Visibility = type == OpenType.View ? Visibility.Collapsed : Visibility.Visible;
            PayList.Items.Add(PayPanels[tempPay.ToString()]);
            SummField.Text =
                (Convert.ToDecimal(
                    Extensions.PrepareStringToConvert(string.IsNullOrEmpty(SummField.Text) ? "0" : SummField.Text)) +
                summ).ToString();

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (tempMonID == 0 || tempOrdID == 0 || Summ.Text=="")
            {
                MessageBox.Show("Заказ, дееньги и сумма должны быть указаны!");
                return;
            }
            var tempPay = new Pay(_connectionSettings, tempMonID, tempOrdID, Convert.ToDecimal(Extensions.PrepareStringToConvert(string.IsNullOrEmpty(Summ.Text) ? "0" : Summ.Text)));
            if (tempPay.o is null || tempPay.m is null)
            {
                return;
            }

            if (Pays.ContainsKey(tempPay.ToString()))
            {
                MessageBox.Show("Такой элемент уже есть в списке!");
                return;
            }

            if ((tempPay.o.Summ - tempPay.o.SummPayed) < tempPay.SummPay)
            {
                MessageBox.Show("Сумма оплаты больше, чем остаток по заказу");
                return;
            }
            if ((tempPay.m.SummRest) < tempPay.SummPay)
            {
                MessageBox.Show("Сумма оплаты больше, чем остаток по счету");
                return;
            }
            if (tempPay.Save(Id))
                Reload();
            
        }
        public Dictionary<string, Pay> Pays = new Dictionary<string, Pay>();
        public Dictionary<string, DockPanel> PayPanels = new Dictionary<string, DockPanel>();

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock dgc = sender as TextBlock;
            var tt = new ToolTip();
            var toolTipPanel = new StackPanel();
            toolTipPanel.Children.Add(new TextBlock { Text = "Сумма должна быть меньше либо равна остатку выплат по заказу и остатка выбранных денег", FontSize = 10, MaxWidth = 500, TextWrapping = TextWrapping.Wrap });
            tt.Content = toolTipPanel;
            if (dgc != null) dgc.ToolTip = tt;
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
            var temp = new Data.Order(_connectionSettings, tempOrdID);
            OrderInfo.Text = "Осталось выплатить: " + (temp.Summ-temp.SummPayed);
            temp = null;
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
            var temp = new Data.Money(_connectionSettings, tempMonID);
            MoneyInfo.Text = "Осталось на счету: " + (temp.SummRest);
            temp = null;
        }
    }
}
