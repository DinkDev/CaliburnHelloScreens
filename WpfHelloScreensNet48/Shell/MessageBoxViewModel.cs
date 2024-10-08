﻿namespace Caliburn.Micro.HelloScreens.Shell
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Framework;

    public class MessageBoxViewModel : Screen, IMessageBox
    {
        private MessageBoxOptions _selection;

        [UsedImplicitly]
        public bool OkVisible => IsVisible(MessageBoxOptions.Ok);

        [UsedImplicitly]
        public bool CancelVisible => IsVisible(MessageBoxOptions.Cancel);

        [UsedImplicitly]
        public bool YesVisible => IsVisible(MessageBoxOptions.Yes);

        [UsedImplicitly]
        public bool NoVisible => IsVisible(MessageBoxOptions.No);

        public string Message { get; set; }

        public MessageBoxOptions Options { get; set; }

        public bool IsAccepted => WasSelected(MessageBoxOptions.Ok) || WasSelected(MessageBoxOptions.Yes);

        [UsedImplicitly]
        public async Task Ok()
        {
            await SelectAsync(MessageBoxOptions.Ok);
        }

        [UsedImplicitly]
        public async Task Cancel()
        {
            await SelectAsync(MessageBoxOptions.Cancel);
        }

        [UsedImplicitly]
        public async Task Yes()
        {
            await SelectAsync(MessageBoxOptions.Yes);
        }

        [UsedImplicitly]
        public async Task No()
        {
            await SelectAsync(MessageBoxOptions.No);
        }

        private bool WasSelected(MessageBoxOptions option)
        {
            return (_selection & option) == option;
        }

        private bool IsVisible(MessageBoxOptions option)
        {
            return (Options & option) == option;
        }

        private async Task SelectAsync(MessageBoxOptions option)
        {
            _selection = option;
            await TryCloseAsync();
        }
    }
}