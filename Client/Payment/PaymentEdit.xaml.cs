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
                
            }
            else
            {
                
            }
            if (type != OpenType.New)
            {
                FillData(id);
            }
        }

        private void FillData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
