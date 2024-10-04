namespace Caliburn.Micro.HelloScreens.Customers
{
    using System;
    using Framework;
    using JetBrains.Annotations;
    using Nito.AsyncEx.Synchronous;

    public class CustomersWorkspaceViewModel : DocumentWorkspace<CustomerViewModel>
    {
        private readonly Func<CustomerViewModel> _customerViewModelFactory;
        private static int _count = 1;

        public CustomersWorkspaceViewModel(Func<CustomerViewModel> customerVmFactory)
        {
            _customerViewModelFactory = customerVmFactory;
        }
        public override string IconName => "Customers";

        public override string Icon => "../Customers/Resources/Images/man1-48.png";

        public override int DisplayOrder { get; } = 1;

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