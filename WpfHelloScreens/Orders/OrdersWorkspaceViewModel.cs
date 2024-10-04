namespace Caliburn.Micro.HelloScreens.Orders
{
    using System;
    using System.ComponentModel.Composition;
    using JetBrains.Annotations;
    using Nito.AsyncEx.Synchronous;
    using Framework;

    [Export(typeof(IWorkspace))]
    public class OrdersWorkspaceViewModel : DocumentWorkspace<OrderViewModel>
    {
        private static int _count = 1;
        private readonly Func<OrderViewModel> _orderViewModelFactory;

        [ImportingConstructor]
        public OrdersWorkspaceViewModel(Func<OrderViewModel> orderVmFactory)
        {
            _orderViewModelFactory = orderVmFactory;
        }

        public override string IconName => "Orders";

        public override string Icon => "../Orders/Resources/Images/shopping-cart-full48.png";

        [UsedImplicitly]
        public void New()
        {
            var vm = _orderViewModelFactory();
            vm.DisplayName = "Order " + _count++;
            vm.IsDirty = true;
            var task = EditAsync(vm);
            task.WaitAndUnwrapException();
        }
    }
}