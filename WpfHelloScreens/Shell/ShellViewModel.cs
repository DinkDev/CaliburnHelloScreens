namespace Caliburn.Micro.HelloScreens.Shell
{
    using System.Collections.Generic;
    using Framework;

    public class ShellViewModel : Conductor<IWorkspace>.Collection.OneActive, IShell
    {
        public ShellViewModel(IDialogManager dialogs, IEnumerable<IWorkspace> workspaces)
        {
            Dialogs = dialogs;
            Items.AddRange(workspaces);
            CloseStrategy = new ApplicationCloseStrategy();
        }

        public IDialogManager Dialogs { get; }
    }
}