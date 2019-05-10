﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using Helpers;
using Helpers.Common;
using Helpers.DB;
using Helpers.Enums;
using Helpers.Interfaces;
using Money;
using Order;
using Payment.Data;

namespace Payment
{
    /// <summary>
    /// Логика взаимодействия для PaymentsWindow.xaml
    /// </summary>
    public partial class PaymentsWindow : Window, IGridWindow
    {
        private readonly ConnectionSettings _connectionSettings;
        private int _CurrentPage, _TotalPages;
        private SqlCommand _BufCommand;
        private int displayIndex;
        private PaymentsFilter Filter;
        private ListSortDirection listSortDirection;
        public PaymentsWindow(ConnectionSettings connectionSettings)
        {
            _connectionSettings = Guard.GetNotNull(connectionSettings, "connectionSettings");
            InitializeComponent();
            Filter = new PaymentsFilter(connectionSettings);
            DataContext = Filter;
            CountPages();
            InitTable();
            SetFiltersToNull();
        }

        private void Ord_click(object sender, RoutedEventArgs e)
        {
            var cds = new OrdersWindow(_connectionSettings, true);
            cds.Owner = this;
            cds.ShowDialog();
            if (TemporaryStorage.Holder.TryGetValue("ID", out string ID))
            {
                SetFiltersToNull();
                Filter.OrderNumFilterText = ID;
                CountPages();
                var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                listSortDirection = Order.SelectedIndex == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
                _BufCommand = new SqlCommand(DBHelper.FormSqlGrid(displayIndex, listSortDirection, Filter._filters, Filter.TableSourceForSort, SqlCommands.GridPart1, SqlCommands.GridByPagePart2), _connection);
                _BufCommand.CommandTimeout = 30;
                UpdateGrid();
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
                SetFiltersToNull();
                Filter.MoneyNumFilterText = ID;
                CountPages();
                var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                listSortDirection = Order.SelectedIndex == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
                _BufCommand = new SqlCommand(DBHelper.FormSqlGrid(displayIndex, listSortDirection, Filter._filters, Filter.TableSourceForSort, SqlCommands.GridPart1, SqlCommands.GridByPagePart2), _connection);
                _BufCommand.CommandTimeout = 30;
                UpdateGrid();
            }
            TemporaryStorage.Holder.Remove("ID");
        }

        public void InitTable()
        {
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            displayIndex = 1;
            listSortDirection = Order.SelectedIndex == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            _BufCommand = new SqlCommand(DBHelper.FormSqlGrid(displayIndex, listSortDirection, Filter._filters, Filter.TableSourceForSort, SqlCommands.GridPart1, SqlCommands.GridByPagePart2), _connection);
            _BufCommand.CommandTimeout = 30;
            Style rowStyle = new Style(typeof(DataGridRow));
            rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                new MouseButtonEventHandler(Row_DoubleClick)));
            UserGrid.RowStyle = rowStyle;
            UserGrid.AutoGeneratedColumns += (sender, args) =>
            {
                UserGrid.Columns[0].Visibility = Visibility.Collapsed;
            };
            UpdateGrid();
        }

        public void CountPages()
        {
            _CurrentPage = 1;
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            using (var command = new SqlCommand(DBHelper.CountTotalPagesByFilter(SqlCommands.CountForPages, Filter._filters), _connection))
            {
                _connection.Open();
                int temp = Convert.ToInt32(command.ExecuteScalar());
                TotalRows.Content = temp;
                _connection.Close();
                _TotalPages = (temp == 0 || temp < 101) ? 1 : (temp % 100 != 0 ? (temp / 100) + 1 : temp / 100);
            }
            UpdatePageCount();
        }

        public void UpdatePageCount()
        {
            CurrentPage.Content = _CurrentPage;
            TotalPages.Content = _TotalPages;
        }

        public void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row)
            {
                var edt = new PaymentEdit(_connectionSettings, OpenType.View, Convert.ToInt32(((DataRowView)row.Item).Row.ItemArray[1].ToString()));
                edt.Owner = this;
                edt.ShowDialog();
                UpdatePageCount();
                UpdateGrid();
            }
        }

        public void SetFiltersToNull()
        {
            Filter.NumberFilterText = "";
            Filter.SummFilterText = "";
            Filter.MoneyNumFilterText = "";
            Filter.OrderNumFilterText = "";
        }

        public void UpdateGrid()
        {
            if (!_BufCommand.Parameters.Contains("@RowStart"))
            {
                _BufCommand.Parameters.Add("@RowStart", SqlDbType.Int);
            }
            if (!_BufCommand.Parameters.Contains("@RowEnd"))
            {
                _BufCommand.Parameters.Add("@RowEnd", SqlDbType.Int);
            }
            _BufCommand.Parameters["@RowStart"].Value = (_CurrentPage - 1) * 100 + 1;
            _BufCommand.Parameters["@RowEnd"].Value = (_CurrentPage - 1) * 100 + 100;
            _BufCommand.CommandTimeout = 30;


            //Progress.Start();
            SqlDataAdapter sda = new SqlDataAdapter(_BufCommand);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            UserGrid.ItemsSource = dt.DefaultView;
            //Progress.Finish();
        }

        private void Clear_Filter_Click(object sender, RoutedEventArgs e)
        {
            SetFiltersToNull();
            Filter_Click(sender, e);
        }

        public void Filter_Click(object sender, RoutedEventArgs e)
        {
            CountPages();
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            displayIndex = 1;
            listSortDirection = Order.SelectedIndex == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            _BufCommand = new SqlCommand(DBHelper.FormSqlGrid(displayIndex, listSortDirection, Filter._filters, Filter.TableSourceForSort, SqlCommands.GridPart1, SqlCommands.GridByPagePart2), _connection);
            _BufCommand.CommandTimeout = 30;
            UpdateGrid();
        }

        public void Grid_Sort(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
            CountPages();
            var _connection = new SqlConnection(_connectionSettings.ConnectionString);
            displayIndex = e.Column.DisplayIndex;
            listSortDirection = Order.SelectedIndex == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            _BufCommand = new SqlCommand(DBHelper.FormSqlGrid(displayIndex, listSortDirection, Filter._filters, Filter.TableSourceForSort, SqlCommands.GridPart1, SqlCommands.GridByPagePart2), _connection);
            _BufCommand.CommandTimeout = 30;
            UpdateGrid();
        }

        public void NextPage_Click(object sender, RoutedEventArgs e)
        {
            if (++_CurrentPage > _TotalPages)
            {
                _CurrentPage = 1;
            }
            UpdatePageCount();
            UpdateGrid();
        }

        public void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            if (--_CurrentPage == 0)
            {
                _CurrentPage = _TotalPages;
            }
            UpdatePageCount();
            UpdateGrid();
        }

        private void PreviewTextInputOnlyNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Extensions.IsTextAllowed(e.Text);
        }

        private void TextBoxPastingOnlyNumbers(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (Extensions.IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var edt = new PaymentEdit(_connectionSettings, OpenType.New);
            edt.Owner = this;
            edt.ShowDialog();
            UpdatePageCount();
            UpdateGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem != null)
            {
                DataRowView row = UserGrid.SelectedItem as DataRowView;
                var edt = new PaymentEdit(_connectionSettings, OpenType.Edit, Convert.ToInt32(row.Row.ItemArray[1]));
                edt.Owner = this;
                edt.ShowDialog();
                UpdatePageCount();
                UpdateGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (UserGrid.SelectedItem != null)
            {
                DataRowView row = UserGrid.SelectedItem as DataRowView;
                if (MessageBox.Show(
                        "Вы уверены, что хотите удалить платеж " + row.Row.ItemArray[1].ToString() + "?",
                        "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var _connection = new SqlConnection(_connectionSettings.ConnectionString);
                    using (var command = new SqlCommand(SqlCommands.DeleteCard, _connection))
                    {
                        if (!command.Parameters.Contains("@DelID"))
                        {
                            command.Parameters.AddWithValue("@DelID", Convert.ToInt32(row.Row.ItemArray[1]));
                        }

                        _connection.Open();
                        if (0 == command.ExecuteNonQuery())
                        {
                            MessageBox.Show("Произошла ошибка при удалении");
                        }
                        _connection.Close();
                    }
                    UpdatePageCount();
                    UpdateGrid();
                }
            }
        }

        
    }
}