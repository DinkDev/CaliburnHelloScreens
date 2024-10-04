namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.Threading.Tasks;

    using Nito.AsyncEx.Synchronous;

    public class ApplicationCloseCheck : IResult
    {
        private readonly IChild _screen;
        private readonly Func<IDialogManager, Task<bool>> _closeCheckAsync;

        public ApplicationCloseCheck(IShell shell, IChild screen, Func<IDialogManager, Task<bool>> closeCheckAsync)
        {
            Shell = shell;
            _screen = screen;
            _closeCheckAsync = closeCheckAsync;
        }

        public IShell Shell { get; }

        public void Execute(CoroutineExecutionContext context)
        {
            if (_screen.Parent is IDocumentWorkspace documentWorkspace)
            {
                var task = documentWorkspace.EditAsync(_screen);
                task.WaitAndUnwrapException();
            }

            var task2 = _closeCheckAsync(Shell.Dialogs);
            var result = task2.WaitAndUnwrapException();
            Completed(this, new ResultCompletionEventArgs { WasCancelled = !result });
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}