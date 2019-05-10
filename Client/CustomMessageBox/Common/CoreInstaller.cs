using Castle.Core;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Helpers.DB;
using Helpers.Settings;
using System.Configuration;

namespace Helpers.Common
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register<SqlConnectionFactory>();
            container.Register<SqlConnectionStringFactory>();
            container.Register(
                LifestyleType.Singleton,
                () => container.Resolve<SqlConnectionFactory>().CreateConnection());

            container.Register(
                LifestyleType.Singleton,
                () => container.Resolve<SqlConnectionStringFactory>().CreateConnectionString());
            container.Register<DbEntityInit>();
            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IGenericFactory>().AsFactory());
            container.Register<PipesSettingsReader>();
            container.Register<ConnectionSettings>(LifestyleType.Singleton);
            container.RegisterInstance(LauncherSettingsReader.ReadFromSettings());
            container.RegisterInstance(CommonSettingsReader.ReadFromSettings());

            container.Register(LifestyleType.Singleton,
                () =>
                {
                    var launcherSettings = container.Resolve<LauncherSettings>();
                    return new XmlSettings(launcherSettings.PathToAppSettings);
                });
        }
    }
}
