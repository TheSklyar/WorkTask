using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Helpers.Logon
{
    /// <summary>
    /// Логика взаимодействия для LogonDlg.xaml
    /// </summary>
    public partial class LogonDlg : Window
    {
        public string AppName = "";
        public string connectionString = "";
        private const string SqlStrConnection = "server={0};uid={1};pwd={2};database={3};Application Name={4}";
        private const string WinStrConnection = "server={0};database={1};Integrated Security=true;Application Name={2}";
        public LogonDlg()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            password.Password = "";
            connectionString = "";
            if (File.Exists(this.path.Text))
                this.SetAlias(this.path.Text, this.Alias.Text);
            if (string.IsNullOrWhiteSpace(this.username.Text))
                this.username.Focus();
            else
                this.password.Focus();
        }
        XElement _xElement;
        private void SetAlias(string fileName, string alias)
        {
            this.Alias.Items.Clear();
            try
            {
                this._xElement = XElement.Load(fileName);
                foreach (XAttribute xattribute in (IEnumerable<XAttribute>)this._xElement.Elements((XName)"machine").Attributes((XName)nameof(alias)).OrderBy<XAttribute, string>((Func<XAttribute, string>)(a1 => a1.Value)))
                    this.Alias.Items.Add((object)xattribute.Value);
                this.Alias.SelectedIndex = this.Alias.Items.IndexOf((object)alias.Trim());
            }
            catch (Exception ex)
            {
                CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace, this);
            }
        }

        private void ChangePath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(path.Text))
            {
                openFileDialog.InitialDirectory = new FileInfo(path.Text).DirectoryName;
            }
            if (openFileDialog.ShowDialog() == true)
                this.SetAlias(openFileDialog.FileName, string.Empty);
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            try
            {
                if (this._xElement == null | this.Alias.SelectedItem == null)
                    return;
                foreach (var data in this._xElement.Elements((XName)"machine").Where<XElement>((Func<XElement, bool>)(d => d.Attribute((XName)"alias").Value == this.Alias.SelectedItem.ToString())).Select(d =>
                {
                    var data = new
                    {
                        server = d.Element((XName)"server"),
                        database = d.Element((XName)"database")
                    };
                    return data;
                }).Take(1))
                {
                    empty1 = data.server.Value;
                    empty2 = data.database.Value;
                }
                if (string.IsNullOrWhiteSpace(this.username.Text) & !string.IsNullOrWhiteSpace(this.password.Password))
                {
                    this.username.Focus();
                    throw new Exception("Введите имя.");
                }
                if (string.IsNullOrWhiteSpace(this.password.Password) & !string.IsNullOrWhiteSpace(this.password.Password))
                {
                    this.password.Focus();
                    throw new Exception("Введите пароль.");
                }
                string str;
                if (!string.IsNullOrWhiteSpace(this.password.Password) || !string.IsNullOrWhiteSpace(this.username.Text))
                    str = string.Format("server={0};uid={1};pwd={2};database={3};Application Name={4}", (object)empty1, (object)this.username.Text.Trim(), (object)this.password.Password.Trim(), (object)empty2, (object)this.AppName);
                else
                    str = string.Format("server={0};database={1};Integrated Security=true;Application Name={2}", (object)empty1, (object)empty2, (object)this.AppName);
                this.connectionString = str;
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                CtmMessageBox.Show("Ошибка", ex.Message, ex.StackTrace, this);
            }
        }
    }
}
