namespace Caliburn.Micro.HelloScreens.Orders
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Framework;

    public class OrderViewModel : DocumentBase
    {
        public OrderViewModel(IShell shell, IDialogManager dialogs) : base(shell, dialogs)
        {
        }

        [UsedImplicitly]
        public async Task SaveAsync()
        {
            IsDirty = false;
            await Dialogs.ShowMessageBoxAsync("Your data has been successfully saved.", "Data Saved");
        }
    }
}