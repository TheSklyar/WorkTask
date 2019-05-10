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

namespace Helpers
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBoxWindow.xaml
    /// </summary>
    public partial class CustomMessageBoxWindow : Window
    {
        MessageData data = new MessageData();
        public CustomMessageBoxWindow(string caption, string text, string stack_trace)
        {
            InitializeComponent();
            this.DataContext = data;
            data.Title_Caption = caption;
            data.Text_Caption = text;
            data.Text_Message = stack_trace;
            Message.Visibility = Visibility.Collapsed;
            LabelStack.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_Expand(object sender, RoutedEventArgs e)
        {
            if (Message.Visibility == Visibility.Visible)
            {
                Message.Visibility = Visibility.Collapsed;
                LabelStack.Visibility = Visibility.Collapsed;
            }
            else
            {
                Message.Visibility = Visibility.Visible;
                LabelStack.Visibility = Visibility.Visible;
            }
        }
    }
}
