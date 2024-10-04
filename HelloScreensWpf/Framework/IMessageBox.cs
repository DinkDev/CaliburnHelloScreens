namespace Caliburn.Micro.HelloScreens.Framework
{
    public interface IMessageBox : IScreen
    {
        string Message { get; set; }
        MessageBoxOptions Options { get; set; }
        bool IsAccepted { get; }
    }
}