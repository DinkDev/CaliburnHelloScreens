using System.Threading;
using System.Threading.Tasks;

namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Framework;

    public class ApplicationCloseStrategy : ICloseStrategy<IWorkspace>
    {
        readonly bool _closeConductedItemsWhenConductorCannotClose;

        /// <summary>
        /// Creates an instance of the class.
        /// </summary>
        /// <param name="closeConductedItemsWhenConductorCannotClose">Indicates that even if all conducted items are not closable, those that are should be closed. The default is FALSE.</param>
        public ApplicationCloseStrategy(bool closeConductedItemsWhenConductorCannotClose = false)
        {
            _closeConductedItemsWhenConductorCannotClose = closeConductedItemsWhenConductorCannotClose;
        }


        //IEnumerator<IWorkspace> enumerator;
        //bool finalResult;
        //Action<bool, IEnumerable<IWorkspace>> callback;

        //public void Execute(IEnumerable<IWorkspace> toClose, Action<bool, IEnumerable<IWorkspace>> callback)
        //{
        //    enumerator = toClose.GetEnumerator();
        //    this.callback = callback;
        //    finalResult = true;

        //    Evaluate(finalResult);
        //}

        //void Evaluate(bool result)
        //{
        //    finalResult = finalResult && result;

        //    if (!enumerator.MoveNext() || !result)
        //    {
        //        callback(finalResult, new List<IWorkspace>());
        //    }
        //    else
        //    {
        //        var current = enumerator.Current;
        //        var conductor = current as IConductor;
        //        if (conductor != null)
        //        {
        //            var tasks = conductor.GetChildren()
        //                .OfType<IHaveShutdownTask>()
        //                .Select(x => x.GetShutdownTask())
        //                .Where(x => x != null);

        //            var sequential = new SequentialResult(tasks.GetEnumerator());
        //            sequential.Completed += (s, e) =>
        //            {
        //                if (!e.WasCancelled)
        //                    Evaluate(!e.WasCancelled);
        //            };
        //            sequential.Execute(new CoroutineExecutionContext());
        //        }
        //        else
        //        {
        //            Evaluate(true);
        //        }
        //    }
        //}


        /// <inheritdoc />
        public async Task<ICloseResult<IWorkspace>> ExecuteAsync(IEnumerable<IWorkspace> toClose, CancellationToken cancellationToken = default)
        {
            var closeable = new List<IWorkspace>();
            var closeCanOccur = true;

            foreach (var child in toClose)
            {
                if (child is IGuardClose guard)
                {
                    var canClose = await guard.CanCloseAsync(cancellationToken);

                    if (canClose)
                    {
                        closeable.Add(child);
                    }

                    closeCanOccur = closeCanOccur && canClose;
                }
                else
                {
                    closeable.Add(child);
                }

                if (child is IConductor conductor)
                {
                    var tasks = conductor.GetChildren()
                        .OfType<IHaveShutdownTask>()
                        .Select(x => x.GetShutdownTask())
                        .Where(x => x != null);

                    var sequential = new SequentialResult(tasks.GetEnumerator());
                    sequential.Completed += (s, e) =>
                    {
                        if (e.WasCancelled)
                        {
                            closeCanOccur = false;
                        }
                    };
                    sequential.Execute(new CoroutineExecutionContext());
                }
            }

            if (!this._closeConductedItemsWhenConductorCannotClose && !closeCanOccur)
            {
                closeable.Clear();
            }

            return new CloseResult<IWorkspace>(closeCanOccur, closeable);
        }
    }
}