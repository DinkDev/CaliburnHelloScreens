namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Windows;
    using Customers;
    using Framework;
    using Nito.AsyncEx.Synchronous;
    using Orders;

    public class ScreensBootstrapper : BootstrapperBase
    {
        CompositionContainer _container;

        public ScreensBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new CompositionContainer(
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x))));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<Func<IMessageBox>>(() => _container.GetExportedValue<IMessageBox>());
            batch.AddExportedValue<Func<CustomerViewModel>>(() => _container.GetExportedValue<CustomerViewModel>());
            batch.AddExportedValue<Func<OrderViewModel>>(() => _container.GetExportedValue<OrderViewModel>());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container
                .GetExportedValues<object>(contract)
                .ToArray();

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var task = DisplayRootViewForAsync<ShellViewModel>();
            task.WaitAndUnwrapException();
        }
    }
}