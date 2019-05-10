using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        
    }
        public Dictionary<string, Pay> Pays = new Dictionary<string, Pay>();
        public Dictionary<string, DockPanel> PayPanels = new Dictionary<string, DockPanel>();
    }
}
