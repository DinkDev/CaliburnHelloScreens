namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using Nito.AsyncEx.Synchronous;

    public class ApplicationCloseCheck : IResult
    {
        private readonly IChild _screen;
        private readonly Func<IDialogManager, Task<bool>> _closeCheckAsync;

        public ApplicationCloseCheck(IChild screen, Func<IDialogManager, Task<bool>> closeCheckAsync)
        {
            _screen = screen;
            _closeCheckAsync = closeCheckAsync;
        }

        [Import]
        public IShell Shell { get; set; }

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