namespace Caliburn.Micro.HelloScreens.Orders
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Framework;

    [Export(typeof(OrderViewModel)), PartCreationPolicy(CreationPolicy.NonShared)]

    public class OrderViewModel : DocumentBase
    {
        [UsedImplicitly]
        public async Task SaveAsync()
        {
            IsDirty = false;
            await Dialogs.ShowMessageBoxAsync("Your data has been successfully saved.", "Data Saved");
        }
    }
}