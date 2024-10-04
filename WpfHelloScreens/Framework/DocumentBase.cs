namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;

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
                return await DoCloseCheckAsync(Dialogs);
            }

            return await Task.FromResult(true);
        }

        //public void CanClose(Action<bool> callback)
        //{
        //    if (IsDirty)
        //    {
        //        DoCloseCheck(Dialogs, callback);
        //    }
        //    else
        //    {
        //        callback(true);
        //    }
        //}

        public IResult GetShutdownTask()
        {
            return IsDirty ? new ApplicationCloseCheck(this, DoCloseCheckAsync) : null;
        }

        //protected virtual void DoCloseCheck(IDialogManager dialogs, Action<bool> callback)
        //{
        //    Dialogs.ShowMessageBox(
        //        "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
        //        "Unsaved Data",
        //        MessageBoxOptions.YesNo,
        //        box => callback(box.WasSelected(MessageBoxOptions.Yes))
        //    );
        //}

        private bool _returnState;

        protected virtual async Task<bool> DoCloseCheckAsync(IDialogManager dialogs)
        {
            return await Task.Run(() =>
            {
                _returnState = false;
                Dialogs.ShowMessageBox(
                    "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
                    "Unsaved Data",
                    MessageBoxOptions.YesNo, SetReturnState);
                return _returnState;
            });
        }

        private void SetReturnState(IMessageBox messageBox)
        {
            _returnState = messageBox.WasSelected(MessageBoxOptions.Ok)
                    || messageBox.WasSelected(MessageBoxOptions.Yes);
        }
    }
}