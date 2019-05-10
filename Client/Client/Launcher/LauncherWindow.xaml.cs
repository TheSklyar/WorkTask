using Helpers.Common;
using Helpers.Settings;
using Helpers;
using Helpers.Interfaces;
using Helpers.Logon;
using Helpers.Waiter;
using System;
using Helpers.DB;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для LauncherWindow.xaml
    /// </summary>
    public partial class LauncherWindow : Window, IWindow
    {
        private readonly ConnectionSettings _connectionSettings;
        private readonly IGenericFactory _factory;
        private readonly CommonSettings _settings;
        private IService[] _services;

        public Window parent => this;

        public LauncherWindow(IGenericFactory factory,
            CommonSettings settings,
            ConnectionSettings connectionSettings)
        {
            _factory = Guard.GetNotNull(factory, "factory");
            _settings = Guard.GetNotNull(settings, "settings");
            _connectionSettings = Guard.GetNotNull(connectionSettings, "connectionSettings");
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var processModule = Process.GetCurrentProcess().MainModule;
                CommonSettings.AssVersion = processModule.FileVersionInfo.ProductVersion;

                this.Title = string.Format(
                    "Launcher ",
                    CommonSettings.AssVersion,
                    _settings.AppVersion);
            }
            catch (Exception ex)
            {
                CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace);
            }
        }

        private void MenuItem_Click_Connect(object sender, RoutedEventArgs e)
        {
            var user = _connectionSettings.User;
            var password = _connectionSettings.Password;
            var connectionString = _connectionSettings.ConnectionString;
            var configFile = _connectionSettings.ConfigFile;
            var alias = _connectionSettings.Alias;
            if (LogonDlgHandler.Show(
                ref user,
                ref password,
                ref connectionString,
                ref configFile,
                ref alias,
                _settings.AppName,
                this))
            {
                _connectionSettings.User = user;
                _connectionSettings.Password = password;
                _connectionSettings.ConnectionString = connectionString;
                _connectionSettings.ConfigFile = configFile;
                _connectionSettings.Alias = alias;

                try
                {
                    try
                    {
                        //Progress.Start();

                        UpdateDisconnectStatus();

                        UpdateConnectionString(connectionString);
                        CheckInit();


                        _connectionSettings.WriteToCfg();

                        UpdateConnectStatus();

                        _services = _factory.CreateAll<IService>();

                        FillModules(_services);
                    }
                    finally
                    {
                        //Progress.Finish();
                        Activate();
                    }
                }
                catch (SqlException ex)
                {
                    CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace, this);
                }
                catch (Exception ex)
                {
                    CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace, this);
                }
            }
        }

        private void FillModules(IEnumerable<IService> services)
        {
            listViewServices.Items.Clear();

            if (services != null)
            {
                listViewServices.ItemsSource = services.Where(service => service.IsVisibleToUser);
            }
        }



        private void CheckInit()
        {
            var clearFunc = _factory.Create<DbEntityInit>();

            Func<int> func = clearFunc.Init;

            var spid = DBExtensions.InvokeWithRetriesThrows(func, 10, 500);
        }

        private void UpdateConnectStatus()
        {
            statusDBName.Text = _connectionSettings.DataBase;

            statusServerName.Text = _connectionSettings.Server;

            statusUserName.Text = string.IsNullOrWhiteSpace(_connectionSettings.User)
                ? SystemInformation.UserName
                : _connectionSettings.User;

        }

        private void UpdateConnectionString(string connectionString)
        {
            _factory.Create<SqlConnection>().ConnectionString = connectionString;
        }


        private void UpdateDisconnectStatus()
        {
            listViewServices.Items.Clear();

            statusDBName.Text = @"???";
            statusServerName.Text = @"???";
            statusUserName.Text = @"???";
        }



        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = ((System.Windows.Controls.ListViewItem)sender).Content as Service;
            selected.Execute();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
