namespace Caliburn.Micro.HelloScreens.Shell
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Framework;

    public class MessageBoxViewModel : Screen, IMessageBox
    {
        private MessageBoxButtons _selection;

        [UsedImplicitly]
        public bool OkVisible => IsVisible(MessageBoxButtons.Ok);

        [UsedImplicitly]
        public bool CancelVisible => IsVisible(MessageBoxButtons.Cancel);

        [UsedImplicitly]
        public bool YesVisible => IsVisible(MessageBoxButtons.Yes);

        [UsedImplicitly]
        public bool NoVisible => IsVisible(MessageBoxButtons.No);

        public string Message { get; set; }

        public MessageBoxButtons Options { get; set; }

        public bool IsAccepted => WasSelected(MessageBoxButtons.Ok) || WasSelected(MessageBoxButtons.Yes);

        [UsedImplicitly]
        public async Task Ok()
        {
            await SelectAsync(MessageBoxButtons.Ok);
        }

        [UsedImplicitly]
        public async Task Cancel()
        {
            await SelectAsync(MessageBoxButtons.Cancel);
        }

        [UsedImplicitly]
        public async Task Yes()
        {
            await SelectAsync(MessageBoxButtons.Yes);
        }

        [UsedImplicitly]
        public async Task No()
        {
            await SelectAsync(MessageBoxButtons.No);
        }

        private bool WasSelected(MessageBoxButtons option)
        {
            return (_selection & option) == option;
        }

        private bool IsVisible(MessageBoxButtons option)
        {
            return (Options & option) == option;
        }

        private async Task SelectAsync(MessageBoxButtons option)
        {
            _selection = option;
            await TryCloseAsync();
        }
    }
}