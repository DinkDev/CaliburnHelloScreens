namespace Caliburn.Micro.HelloScreens.Customers
{
    using System;
    using System.ComponentModel.Composition;
    using Framework;
    using Nito.AsyncEx.Synchronous;

    [Export(typeof(IWorkspace))]
    [Export(typeof(CustomersWorkspaceViewModel))]
    public class CustomersWorkspaceViewModel : DocumentWorkspace<CustomerViewModel>
    {
        readonly Func<CustomerViewModel> createCustomerViewModel;
        static int count = 1;

        [ImportingConstructor]
        public CustomersWorkspaceViewModel(Func<CustomerViewModel> customerVMFactory)
        {
            createCustomerViewModel = customerVMFactory;
        }

        public override string IconName
        {
            get { return "Customers"; }
        }

        public override string Icon
        {
            get { return "../Customers/Resources/Images/man1-48.png"; }
        }

        public void New()
        {
            var vm = createCustomerViewModel();
            vm.DisplayName = "Customer " + count++;
            vm.IsDirty = true;
            var task = EditAsync(vm);
            task.WaitAndUnwrapException();
        }
    }
}