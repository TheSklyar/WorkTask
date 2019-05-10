using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Helpers;
using Helpers.Common;
using Helpers.Interfaces;

namespace Payment.Bootstrap
{
    public class PaymentInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Guard.CheckNotNull(container, "container");
            container.Register<PaymentsWindow>();
            container.RegisterInterfacesFromAssembly<IService>(Classes.FromAssemblyInThisApplication());
        }
    }
}
