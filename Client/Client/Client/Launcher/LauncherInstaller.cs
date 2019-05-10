using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Helpers;
using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers.Common;
using Castle.Core;

namespace Client
{
    public class LauncherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Guard.CheckNotNull(container, "container");
            container.Register<IWindow, LauncherWindow>(LifestyleType.Singleton);
            container.RegisterInterfacesFromAssembly<IService>(Classes.FromAssemblyInThisApplication());
        }
    }
}
