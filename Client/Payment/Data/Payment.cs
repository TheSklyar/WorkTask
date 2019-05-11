using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Helpers.Common;

namespace Payment.Data
{
    public class Pay
    {
        public Money m;
        public Order o;
        public decimal SummPay { get; set; }
        public Button Button;
        public override string ToString()
        {
            return oID + "|" + mID;
        }

        private int mID, oID;
        public Pay(ConnectionSettings connectionSettings, int monID, int ordID, decimal summ)
        {
            mID = monID;
            oID = ordID;
            SummPay = summ;
            m = new Money(connectionSettings, monID);
            o = new Order(connectionSettings, ordID);
            if (m.Summ==0 || o.Summ==0)
            {
                MessageBox.Show("Попытка добавить деньги или заказ с нулевой суммой!");
                m = null;
                o = null;
            }
        }
        public DockPanel Create()
        {
            Button = new Button
            {
                FontSize = 12,
                Margin = new Thickness(0, 0, 5, 0),
                Content = "Удалить"
            };
            var TextBlock = new TextBlock
            {
                FontSize = 12,
                Text = m.ToString() + "\t|\t"+ o.ToString() + "\t|\t" + SummPay.ToString(),
                VerticalAlignment = VerticalAlignment.Center
            };
            var DockPanel = new DockPanel { Margin = new Thickness(2) };
            DockPanel.Children.Add(Button);
            DockPanel.Children.Add(TextBlock);
            return DockPanel;
        }
    }
}
