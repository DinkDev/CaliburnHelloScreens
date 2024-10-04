namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections;
    using System.Dynamic;
    using System.Threading;
    using System.Threading.Tasks;
    using Framework;
    using Nito.AsyncEx.Synchronous;

    public class DialogConductorViewModel : PropertyChangedBase, IDialogManager, IConductActiveItem
    {
        private readonly IWindowManager _windowManager;
        private readonly Func<IMessageBox> _messageBoxFactory;

        public DialogConductorViewModel(IWindowManager windowManager, Func<IMessageBox> messageBoxFactory)
        {
            _windowManager = windowManager;
            _messageBoxFactory = messageBoxFactory;
        }

        public IScreen ActiveItem { get; set; }

        public IEnumerable GetChildren()
        {
            return ActiveItem != null ? new[] { ActiveItem } : Array.Empty<object>();
        }

        public async Task ActivateItemAsync(object item, CancellationToken cancellationToken = new CancellationToken())
        {
            if (item is IChild child)
            {
                child.Parent = this;
            }

            ActiveItem = item as IScreen;

            if (ActiveItem != null)
            {
                await ActiveItem.ActivateAsync(cancellationToken);
            }

            NotifyOfPropertyChange(() => ActiveItem);
            ActivationProcessed(this, new ActivationProcessedEventArgs { Item = ActiveItem, Success = true });
        }

        public async Task DeactivateItemAsync(object item, bool close, CancellationToken cancellationToken = new CancellationToken())
        {
            if (item is IGuardClose guard)
            {
                var result = await guard.CanCloseAsync(CancellationToken.None);
                if (result)
                {
                    await CloseActiveItemCoreAsync();
                }
            }
            else
            {
                await CloseActiveItemCoreAsync();
            }
        }

        object IHaveActiveItem.ActiveItem
        {
            get => ActiveItem;
            set
            {
                var task = ActivateItemAsync(value);
                task.WaitAndUnwrapException();
            }
        }

        public event EventHandler<ActivationProcessedEventArgs> ActivationProcessed = delegate { };

        public void ShowDialog(IScreen dialogModel)
        {
            var task = ActivateItemAsync(dialogModel);
            task.WaitAndUnwrapException();
        }

        public async Task<bool> ShowMessageBoxAsync(string message, string title = "", MessageBoxOptions options = MessageBoxOptions.Ok)
        {
            var box = _messageBoxFactory();

            box.DisplayName = title;
            box.Options = options;
            box.Message = message;

            dynamic settings = new ExpandoObject();
            settings.WindowStyle = System.Windows.WindowStyle.ToolWindow;

            await _windowManager.ShowDialogAsync(box, settings:settings);

            return box.IsAccepted;
        }

        private async Task CloseActiveItemCoreAsync()
        {
            var oldItem = ActiveItem;
            await ActivateItemAsync(null);
            await oldItem.DeactivateAsync(true);
        }
    }
}