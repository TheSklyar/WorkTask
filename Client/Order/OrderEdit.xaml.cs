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
using Order.Data;

namespace Order
{
    /// <summary>
    /// Логика взаимодействия для OrderEdit.xaml
    /// </summary>
    public partial class OrderEdit : Window
    {
        //Не стал городить биндинги, так как слишком простое окно
        private decimal oldSumm=0, oldPayed=0;
        private ConnectionSettings _connectionSettings;
        private OpenType type;
        public OrderEdit(ConnectionSettings connectionSettings, OpenType type, int id=0)
        {
            _connectionSettings = connectionSettings;
            this.type = type;
            InitializeComponent();
            if (type == OpenType.View)
            {
                Save.Visibility = Visibility.Hidden;
                ID.IsReadOnly = true;
                DateVal.IsEnabled = false;
                SummVal.IsReadOnly = true;
                SummValPayed.IsReadOnly = true;
            }
            else
            {
                Save.Visibility = Visibility.Visible;
                ID.IsReadOnly = true;
                DateVal.IsEnabled = true;
                SummVal.IsReadOnly = false;
                SummValPayed.IsReadOnly = true;
            }
            if (type != OpenType.New)
            {
                FillData(id);
            }
        }

        private void FillData(int code)
        {
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(SqlCommands.SelectOrderByID, _connection))
            {
                if (!command.Parameters.Contains("@ID"))
                {
                    command.Parameters.AddWithValue("@ID", code);
                }
                _connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ID.Text = code.ToString();
                        DateVal.SelectedDate = reader.GetDateTime(0);
                        oldSumm = reader.GetDecimal(1);
                        SummVal.Text = oldSumm.ToString();
                        oldPayed = reader.GetDecimal(2);
                        SummValPayed.Text = oldPayed.ToString();
                    }
                    else
                    {
                        throw new Exception("Заказ не найден");
                    }
                }
                _connection.Close();
            }
        }

        private void PreviewTextInputOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Extensions.IsTextAllowedDecimal(e.Text);
        }

        private void TextBoxPastingOnlyNumbers(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (Extensions.IsTextAllowedDecimal(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SummVal.Text == "" || DateVal.SelectedDate is null)
            {
                MessageBox.Show("Поля дата и сумма должны быть заполнены!");
                return;
            }
            if (type == OpenType.Edit)
            {
                int count=0;
                var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                using (var command = new SqlCommand(SqlCommands.Update, _connection))
                {
                    if (!command.Parameters.Contains("@ID"))
                    {
                        command.Parameters.AddWithValue("@ID", Convert.ToInt32(ID.Text));
                    }
                    if (!command.Parameters.Contains("@Date"))
                    {
                        command.Parameters.AddWithValue("@Date", DateVal.SelectedDate.Value);
                    }

                    decimal newsumm =
                        Convert.ToDecimal(
                            Extensions.PrepareStringToConvert(string.IsNullOrEmpty(SummVal.Text) ? "0" : SummVal.Text));
                    if (!command.Parameters.Contains("@Summ"))
                    {
                        command.Parameters.AddWithValue("@Summ", newsumm);
                    }

                    if (!command.Parameters.Contains("@SummPayed"))
                    {
                        command.Parameters.AddWithValue("@SummPayed", (oldPayed+(oldSumm - newsumm))<0?0: (oldPayed + (oldSumm - newsumm)));
                    }
                    _connection.Open();
                    count = command.ExecuteNonQuery();
                    _connection.Close();

                }
                if (count == 0)
                {
                    MessageBox.Show("Произошла ошибка, сохранение не удалось!");
                }
                else
                {
                    MessageBox.Show("Успешно сохранено!");
                }
            }
            else
            {
                int count = 0;
                var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                using (var command = new SqlCommand(SqlCommands.SaveNew, _connection))
                {
                    if (!command.Parameters.Contains("@Date"))
                    {
                        command.Parameters.AddWithValue("@Date", DateVal.SelectedDate.Value);
                    }

                    decimal newsumm =
                        Convert.ToDecimal(
                            Extensions.PrepareStringToConvert(string.IsNullOrEmpty(SummVal.Text) ? "0" : SummVal.Text));
                    if (!command.Parameters.Contains("@Summ"))
                    {
                        command.Parameters.AddWithValue("@Summ", newsumm);
                    }

                    if (!command.Parameters.Contains("@SummPayed"))
                    {
                        command.Parameters.AddWithValue("@SummPayed", 0);
                    }
                    _connection.Open();
                    count = command.ExecuteNonQuery();
                    _connection.Close();

                }
                if (count == 0)
                {
                    MessageBox.Show("Произошла ошибка, сохранение не удалось!");
                }
                else
                {
                    MessageBox.Show("Успешно сохранено!");
                    type = OpenType.Edit;
                }
                
            }
        }
    }
}
