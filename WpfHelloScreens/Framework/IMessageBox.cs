namespace Caliburn.Micro.HelloScreens.Framework {
    using System.Threading.Tasks;

    public interface IMessageBox : IScreen {
        string Message { get; set; }
        MessageBoxOptions Options { get; set; }

        Task Ok();
        Task Cancel();
        Task Yes();
        Task No();

        bool WasSelected(MessageBoxOptions option);
    }
}