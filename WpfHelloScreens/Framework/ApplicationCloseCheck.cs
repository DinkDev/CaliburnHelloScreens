namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Nito.AsyncEx.Synchronous;

    public class ApplicationCloseCheck : IResult
    {
        readonly Action<IDialogManager, Action<bool>> closeCheck;
        readonly IChild screen;

        public ApplicationCloseCheck(IChild screen, Action<IDialogManager, Action<bool>> closeCheck)
        {
            this.screen = screen;
            this.closeCheck = closeCheck;
        }

        [Import]
        public IShell Shell { get; set; }

        public void Execute(CoroutineExecutionContext context)
        {
            var documentWorkspace = screen.Parent as IDocumentWorkspace;
            if (documentWorkspace != null)
            {
                var task =documentWorkspace.EditAsync(screen);
                task.WaitAndUnwrapException();
            }

            closeCheck(Shell.Dialogs,
                result => Completed(this, new ResultCompletionEventArgs { WasCancelled = !result }));
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}