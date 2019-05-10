using Helpers;
using Helpers.Common;
using Helpers.Interfaces;
using Helpers.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Order.Bootstrap
{
    public class OrdersService : Service
    {
        public OrdersService(
            IWindow parent,
            IGenericFactory factory)
            
        {
            _parentWindow = Guard.GetNotNull(parent, "nativeWindow");
            _factory = Guard.GetNotNull(factory, "factory");

            Ident = "{29bf447a-17b3-469e-842a-12eefc8e6110}";
            Name = "Заказы";
            Description = string.Format("Создание новых заказов и их изменение");
            IsVisibleToUser = true;
        }

        public override bool Execute()
        {
            return ExecutionShield(() =>
            {
                var orderWindow = _factory.Create<OrdersWindow>();
                orderWindow.Owner = _parentWindow.parent;
                orderWindow.ShowDialog();
                return true;
            });
        }

        private readonly IGenericFactory _factory;
        private readonly IWindow _parentWindow;
    }
}
