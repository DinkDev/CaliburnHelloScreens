namespace Caliburn.Micro.HelloScreens.Shell
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Framework;

    [Export(typeof(IShell))]
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : Conductor<IWorkspace>.Collection.OneActive, IShell
    {
        [ImportingConstructor]
        public ShellViewModel(IDialogManager dialogs, [ImportMany] IEnumerable<IWorkspace> workspaces)
        {
            Dialogs = dialogs;
            Items.AddRange(workspaces);
            CloseStrategy = new ApplicationCloseStrategy();
        }

        public IDialogManager Dialogs { get; }
    }
}