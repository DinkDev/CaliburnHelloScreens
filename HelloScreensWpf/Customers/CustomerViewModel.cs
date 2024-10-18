namespace Caliburn.Micro.HelloScreens.Customers
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Framework;

    public class CustomerViewModel : DocumentBase
    {
        public CustomerViewModel(IShell shell, IDialogManager dialogs) : base(shell, dialogs)
        {
        }

        //[UsedImplicitly]
        //public async Task SaveAsync()
        //{
        //    await Dialogs.SetBusyAsync("Saving...")
        //        .ContinueWith(x => Task.Delay(5000))
        //        .ContinueWith(x => Dialogs.ClearBusyAsync())
        //        .ContinueWith(async x =>
        //        {
        //            IsDirty = false;
        //            await Dialogs.ShowMessageBoxAsync("Your data has been successfully saved.", "Data Saved");
        //        });
        //}

        [UsedImplicitly]
        public async Task SaveAsync()
        {
            IsDirty = false;
            await Dialogs.ShowMessageBoxAsync("Your data has been successfully saved.", "Data Saved");
        }

        [UsedImplicitly]
        public void EditAddress()
        {
            Dialogs.ShowDialog(new AddressViewModel());
        }
    }
}