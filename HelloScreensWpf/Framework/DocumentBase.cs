namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DocumentBase : Screen, IHaveShutdownTask
    {
        public DocumentBase(IShell shell, IDialogManager dialogs)
        {
            Shell = shell;
            Dialogs = dialogs;
        }

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

        public IShell Shell { get; }

        public IDialogManager Dialogs { get; }

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
            return IsDirty ? new ApplicationCloseCheck(Shell,this, DoCloseCheckAsync) : null;
        }

        protected virtual async Task<bool> DoCloseCheckAsync(IDialogManager dialogs)
        {
            return await Dialogs.ShowMessageBoxAsync(
                "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
                "Unsaved Data",
                MessageBoxButtons.YesNo);
        }
    }
}