namespace Caliburn.Micro.HelloScreens.Customers
{
    using System;
    using System.ComponentModel.Composition;
    using Framework;
    using JetBrains.Annotations;
    using Nito.AsyncEx.Synchronous;

    [Export(typeof(IWorkspace))]
    [Export(typeof(CustomersWorkspaceViewModel))]
    public class CustomersWorkspaceViewModel : DocumentWorkspace<CustomerViewModel>
    {
        private readonly Func<CustomerViewModel> _customerViewModelFactory;
        private static int _count = 1;

        [ImportingConstructor]
        public CustomersWorkspaceViewModel(Func<CustomerViewModel> customerVmFactory)
        {
            _customerViewModelFactory = customerVmFactory;
        }
        public override string IconName => "Customers";

        public override string Icon => "../Customers/Resources/Images/man1-48.png";

        [UsedImplicitly]
        public void New()
        {
            var vm = _customerViewModelFactory();
            vm.DisplayName = "Customer " + _count++;
            vm.IsDirty = true;
            var task = EditAsync(vm);
            task.WaitAndUnwrapException();
        }
    }
}