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

namespace Order
{
    /// <summary>
    /// Логика взаимодействия для OrderEdit.xaml
    /// </summary>
    public partial class OrderEdit : Window
    {
        private ConnectionSettings _connectionSettings;
        private OpenType @new;

        public OrderEdit(ConnectionSettings connectionSettings, OpenType @new)
        {
            _connectionSettings = connectionSettings;
            this.@new = @new;
        }

        public OrderEdit(Helpers.Common.ConnectionSettings _connectionSettings, Helpers.Enums.OpenType view, int v)
        {
            InitializeComponent();
        }
    }
}
