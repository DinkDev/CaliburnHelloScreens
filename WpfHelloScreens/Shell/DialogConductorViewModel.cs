namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using Framework;
    using Nito.AsyncEx.Synchronous;

    [Export(typeof(IDialogManager)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class DialogConductorViewModel : PropertyChangedBase, IDialogManager, IConductActiveItem
    {
        readonly Func<IMessageBox> createMessageBox;

        [ImportingConstructor]
        public DialogConductorViewModel(Func<IMessageBox> messageBoxFactory)
        {
            createMessageBox = messageBoxFactory;
        }

        public IScreen ActiveItem { get; set; }

        public IEnumerable GetChildren()
        {
            return ActiveItem != null ? new[] { ActiveItem } : new object[0];
        }

        //public void ActivateItem(object item)
        //{
        //    ActiveItem = item as IScreen;

        //    if (ActiveItem is IChild child)
        //    {
        //        child.Parent = this;
        //    }

        //    ActiveItem?.ActivateAsync();

        //    NotifyOfPropertyChange(() => ActiveItem);
        //    ActivationProcessed(this, new ActivationProcessedEventArgs { Item = ActiveItem, Success = true });
        //}

        public async Task ActivateItemAsync(object item, CancellationToken cancellationToken = new CancellationToken())
        {
            if (item is IChild child)
            {
                child.Parent = this;
            }

            ActiveItem = item as IScreen;

            if (ActiveItem != null)
            {
                await ActiveItem.ActivateAsync(cancellationToken).ConfigureAwait(false);
            }

            NotifyOfPropertyChange(() => ActiveItem);
            ActivationProcessed(this, new ActivationProcessedEventArgs { Item = ActiveItem, Success = true });
        }

        //public void DeactivateItem(object item, bool close)
        //{
        //    if (item is IGuardClose guard)
        //    {
        //        //guard.CanClose(result =>
        //        //{
        //        //    if (result)
        //        //    {
        //        //        CloseActiveItemCore();
        //        //    }
        //        //});
        //
        //        var result = guard.CanCloseAsync(CancellationToken.None).Result;
        //        if (result)
        //        {
        //            CloseActiveItemCore();
        //        }
        //    }
        //    else
        //    {
        //        CloseActiveItemCore();
        //    }
        //}

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

        public void ShowMessageBox(string message, string title = "Hello Screens",
            MessageBoxOptions options = MessageBoxOptions.Ok, Action<IMessageBox> callback = null)
        {
            var box = createMessageBox();

            box.DisplayName = title;
            box.Options = options;
            box.Message = message;

            if (callback != null)
            {
                box.Deactivated += async (sender, args) => { await Task.Run(() => callback(box)); };
            }

            var task = ActivateItemAsync(box);
            task.WaitAndUnwrapException();
        }

        //void CloseActiveItemCore() {
        //    var oldItem = ActiveItem;
        //    ActivateItem(null);
        //    oldItem.Deactivate(true);
        //}

        private async Task CloseActiveItemCoreAsync()
        {
            var oldItem = ActiveItem;
            await ActivateItemAsync(null);
            await oldItem.DeactivateAsync(true);
        }
    }
}