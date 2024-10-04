namespace Caliburn.Micro.HelloScreens.Customers
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Framework;
    using JetBrains.Annotations;

    [Export(typeof(CustomerViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerViewModel : DocumentBase
    {
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