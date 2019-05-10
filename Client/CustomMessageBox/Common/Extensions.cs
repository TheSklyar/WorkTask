using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core;
using Castle.MicroKernel.ModelBuilder.Descriptors;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Helpers.Common
{
    public static class Extensions
    {
        public static string ReadConfigValue(this NameValueCollection config, string paramName)
        {
            var configValue = Guard.GetNotNull(config, "config")[paramName];
            Guard.CheckContainsText(configValue, paramName);
            return configValue;
        }

        public static string PrepareStringToConvert(string input)
        {
            return input.Replace(" ", "").Replace(',', '.').
                Replace('.', System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]);
        }

        private const LifestyleType DefaultLifestyleType = LifestyleType.Transient;
        public static void Register<TImplementation>(
            this IWindsorContainer container,
            LifestyleType lifestyle = DefaultLifestyleType,
            Func<TImplementation> factoryMethod = null)
            where TImplementation : class
        {
            Guard.CheckNotNull(container, "container");

            var component = CreateComponent<TImplementation>(lifestyle);

            if (factoryMethod != null)
            {
                component.UsingFactoryMethod(factoryMethod);
            }

            container.Register(component);
        }
        private static ComponentRegistration<TInterface> CreateComponent<TInterface, TImplementation>(LifestyleType lifestyle)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            return Component
                .For<TInterface>()
                .ImplementedBy<TImplementation>()
                .AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle));
        }

        public static void RegisterInstance<TInterface>(this IWindsorContainer container, TInterface instance)
            where TInterface : class
        {
            Guard.CheckNotNull(container, "container");
            Guard.CheckNotNull(instance, "instance");

            container.Register(
                Component
                    .For<TInterface>()
                    .Instance(instance));
        }
        public static void Register<TInterface, TImplementation>(
            this IWindsorContainer container,
            LifestyleType lifestyle = DefaultLifestyleType,
            Func<TImplementation> factoryMethod = null)
            where TImplementation : class, TInterface
            where TInterface : class
        {
            Guard.CheckNotNull(container, "container");

            var component = CreateComponent<TInterface, TImplementation>(lifestyle);

            if (factoryMethod != null)
            {
                component.UsingFactoryMethod(factoryMethod);
            }

            container.Register(component);
        }

        private static ComponentRegistration<TImplementation> CreateComponent<TImplementation>(LifestyleType lifestyle)
            where TImplementation : class
        {
            return Component
                .For<TImplementation>()
                .AddDescriptor(new LifestyleDescriptor<TImplementation>(lifestyle));
        }

        public static void RegisterInterfacesFromAssembly<TInterface>(
            this IWindsorContainer container,
            FromAssemblyDescriptor assemblyDescriptor = null,
            LifestyleType lifestyle = DefaultLifestyleType)
            where TInterface : class
        {
            Guard.CheckNotNull(container, "container");

            if (assemblyDescriptor == null)
            {
                assemblyDescriptor = Classes.FromThisAssembly();
            }

            container.Register(assemblyDescriptor
                .BasedOn<TInterface>()
                .WithService.AllInterfaces()
                .WithService.Self()
                .Configure(x => x.AddDescriptor(new LifestyleDescriptor<TInterface>(lifestyle))));
        }

        private static readonly Regex _regex = new Regex("^[0-9]+$");
        public static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }

    
}
