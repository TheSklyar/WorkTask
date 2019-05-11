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

namespace Money.Bootstrap
{
    public class MoneyService : Service
    {
        public MoneyService(
            IWindow parent,
            IGenericFactory factory)

        {
            _parentWindow = Guard.GetNotNull(parent, "nativeWindow");
            _factory = Guard.GetNotNull(factory, "factory");

            Ident = "{1934d486-3fab-41c4-93e2-43488dde73f1}";
            Name = "Счета";
            Description = string.Format("Создание нового счета и изменение");
            IsVisibleToUser = true;
        }

        public override bool Execute()
        {
            return ExecutionShield(() =>
            {
                var orderWindow = _factory.Create<MoneyWindow>();
                orderWindow.Owner = _parentWindow.parent;
                orderWindow.ShowDialog();
                return true;
            });
        }

        private readonly IGenericFactory _factory;
        private readonly IWindow _parentWindow;
    }
}