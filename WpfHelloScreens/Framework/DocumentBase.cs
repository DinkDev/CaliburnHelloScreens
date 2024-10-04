﻿namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;

    public class DocumentBase : Screen, IHaveShutdownTask {
        bool _isDirty;

        public bool IsDirty {
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

        public void CanClose(Action<bool> callback)
        {
            if (IsDirty)
            {
                DoCloseCheck(Dialogs, callback);
            }
            else
            {
                callback(true);
            }
        }

        public IResult GetShutdownTask() {
            return IsDirty ? new ApplicationCloseCheck(this, DoCloseCheck) : null;
        }

        protected virtual void DoCloseCheck(IDialogManager dialogs, Action<bool> callback)
        {
            Dialogs.ShowMessageBox(
                "You have unsaved data. Are you sure you want to close this document? All changes will be lost.",
                "Unsaved Data",
                MessageBoxOptions.YesNo,
                box => callback(box.WasSelected(MessageBoxOptions.Yes))
                );
        }
    }
}