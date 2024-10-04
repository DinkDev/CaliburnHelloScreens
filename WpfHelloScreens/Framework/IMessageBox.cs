namespace Caliburn.Micro.HelloScreens.Framework
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IMessageBox : IScreen
    {
        string Message { get; set; }
        MessageBoxOptions Options { get; set; }

        [UsedImplicitly]
        Task Ok();
        [UsedImplicitly]
        Task Cancel();
        [UsedImplicitly]
        Task Yes();
        Task No();

        bool WasSelected(MessageBoxOptions option);
    }
}