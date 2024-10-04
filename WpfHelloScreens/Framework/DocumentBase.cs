namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using Nito.AsyncEx.Synchronous;

    public class DocumentBase : Screen, IHaveShutdownTask
    {
        bool _isDirty;

        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (value != _isDirty)
                {
                    _isDirty = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        [Import]
        public IDialogManager Dialogs { get; set; }

        // TODO: is this necessary

        public override async Task<bool> CanCloseAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (IsDirty)
            {
                // return await DoCloseCheckAsync(Dialogs);

                return await DoCloseCheckAsync(Dialogs);
            }

            return await Task.FromResult(true);
        }

        public IResult GetShutdownTask()
        {
            return IsDirty ? new ApplicationCloseCheck(this, DoCloseCheckAsync) : null;
        }

        protected virtual async Task<bool> DoCloseCheckAsync(IDialogManager dialogs)
        {
            return await Dialogs.ShowMessageBoxAsync(
                "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
                "Unsaved Data",
                MessageBoxOptions.YesNo);
        }

        //protected virtual async Task<IMessageBox> DoCloseCheckNewAsync(IDialogManager dialogs)
        //{
        //    return await Dialogs.ShowMessageBoxAsync(
        //        "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
        //        "Unsaved Data",
        //        MessageBoxOptions.YesNo);
        //}
    }
}