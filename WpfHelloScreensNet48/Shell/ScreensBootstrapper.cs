
using System.Reflection;

namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using Autofac;
    using Nito.AsyncEx.Synchronous;

    public class ScreensBootstrapper : BootstrapperBase
    {
        private IContainer _container;

        public ScreensBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new ContainerBuilder();

            // register types generally
            var local = Assembly.GetExecutingAssembly();
            var caliburn = Assembly.GetAssembly(typeof(WindowManager))
                           ?? throw new NullReferenceException(nameof(Caliburn));

            builder.RegisterAssemblyTypes(local, caliburn)
                .AsSelf()
                .AsImplementedInterfaces();

            // ensure only one Shell
            builder.RegisterType<ShellViewModel>()
                .SingleInstance()
                .AsImplementedInterfaces();

            _container = builder.Build();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(serviceType))
                {
                    return _container.Resolve(serviceType);
                }
            }
            else if (_container.IsRegisteredWithKey(key, serviceType))
            {
                return _container.ResolveKeyed(key, serviceType);
            }

            var keyMessage = string.IsNullOrWhiteSpace(key) ? string.Empty : $"Key: {key}, ";
            var msg = $"Unable to find registration for {keyMessage} Service: {serviceType.Name}.";
            throw new Exception(msg);
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var type = typeof(IEnumerable<>).MakeGenericType(serviceType);
            return _container.Resolve(type) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectUnsetProperties(instance);

            // alternatively, could use InjectProperties to inject every property - set or not
            //_container.InjectProperties(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var task = DisplayRootViewForAsync<ShellViewModel>();
            task.WaitAndUnwrapException();
        }
    }
}