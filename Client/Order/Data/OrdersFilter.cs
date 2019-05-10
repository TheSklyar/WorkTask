using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Common;

namespace Order.Data
{
    public class OrdersFilter : INotifyPropertyChanged
    {
        ConnectionSettings _connectionSettings;
        public Dictionary<int, string> _filters;
        public OrdersFilter(ConnectionSettings connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _filters = new Dictionary<int, string>
            {
                {1, ""},
                {2, ""},
                {3, ""}
            };
        }

        public Dictionary<int, string> TableSourceForSort = new Dictionary<int, string>
        {
            {1,@"tor.ID" },
            {2,@"tor.[Date]" },
            {3,@"tor.[Summ]" },
            {4,@"tor.[SummPayed]" }
        };

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
