namespace Caliburn.Micro.HelloScreens.Shell
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Framework;
    using Nito.AsyncEx.Synchronous;

    [Export(typeof(IMessageBox)), PartCreationPolicy(CreationPolicy.NonShared)]

    public class MessageBoxViewModel : Screen, IMessageBox
    {
        MessageBoxOptions selection;

        public bool OkVisible
        {
            get { return IsVisible(MessageBoxOptions.Ok); }
        }

        public bool CancelVisible
        {
            get { return IsVisible(MessageBoxOptions.Cancel); }
        }

        public bool YesVisible
        {
            get { return IsVisible(MessageBoxOptions.Yes); }
        }

        public bool NoVisible
        {
            get { return IsVisible(MessageBoxOptions.No); }
        }

        public string Message { get; set; }
        public MessageBoxOptions Options { get; set; }

        public async Task Ok()
        {
            await SelectAsync(MessageBoxOptions.Ok);
        }

        public async Task Cancel()
        {
            await SelectAsync(MessageBoxOptions.Cancel);
        }

        public async Task Yes()
        {
            await SelectAsync(MessageBoxOptions.Yes);
        }

        public async Task No()
        {
            await SelectAsync(MessageBoxOptions.No);
        }

        public bool WasSelected(MessageBoxOptions option)
        {
            return (selection & option) == option;
        }

        bool IsVisible(MessageBoxOptions option)
        {
            return (Options & option) == option;
        }

        private async Task SelectAsync(MessageBoxOptions option)
        {
            selection = option;
            await TryCloseAsync();
        }
    }
}