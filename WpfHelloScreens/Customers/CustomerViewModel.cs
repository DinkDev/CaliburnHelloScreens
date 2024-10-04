namespace Caliburn.Micro.HelloScreens.Customers
{
    using System.ComponentModel.Composition;
    using Framework;
    using JetBrains.Annotations;

    [Export(typeof(CustomerViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CustomerViewModel : DocumentBase
    {
        [UsedImplicitly]
        public void Save()
        {
            IsDirty = false;
            Dialogs.ShowMessageBox("Your data has been successfully saved.", "Data Saved");
        }

        [UsedImplicitly]
        public void EditAddress()
        {
            Dialogs.ShowDialog(new AddressViewModel());
        }
    }
}