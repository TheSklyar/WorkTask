using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using Helpers.Common;
using Helpers.Interfaces;

namespace Payment.Bootstrap
{
    public class PaymentService : Service
    {
        public PaymentService(
            IWindow parent,
            IGenericFactory factory)

        {
            _parentWindow = Guard.GetNotNull(parent, "nativeWindow");
            _factory = Guard.GetNotNull(factory, "factory");

            Ident = "{c8886abb-be37-4759-87b9-7a5161156e79}";
            Name = "Платежи";
            Description = string.Format("Работа с платежами");
            IsVisibleToUser = true;
        }

        public override bool Execute()
        {
            return ExecutionShield(() =>
            {
                var orderWindow = _factory.Create<PaymentsWindow>();
                orderWindow.Owner = _parentWindow.parent;
                orderWindow.ShowDialog();
                return true;
            });
        }

        private readonly IGenericFactory _factory;
        private readonly IWindow _parentWindow;
    }
}