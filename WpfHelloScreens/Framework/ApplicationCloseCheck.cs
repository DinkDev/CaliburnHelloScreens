namespace Caliburn.Micro.HelloScreens.Framework
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    using Nito.AsyncEx.Synchronous;

    public class ApplicationCloseCheck : IResult
    {
        private readonly IChild _screen;
        //private readonly Action<IDialogManager, Action<bool>> _closeCheck;
        private readonly Func<IDialogManager, Task<bool>> _closeCheckAsync;

        //public ApplicationCloseCheck(IChild screen, Action<IDialogManager, Action<bool>> closeCheck)
        //{
        //    _screen = screen;
        //    _closeCheck = closeCheck;
        //}

        public ApplicationCloseCheck(IChild screen, Func<IDialogManager, Task<bool>> closeCheckAsync)
        {
            _screen = screen;
            _closeCheckAsync = closeCheckAsync;
        }

        public ApplicationCloseCheck(IChild screen, Func<IDialogManager, Task<bool>> closeCheckAsync, bool v)
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