namespace Caliburn.Micro.HelloScreens.Framework
{
    public interface IMessageBox : IScreen
    {
        string Message { get; set; }
        MessageBoxButtons Options { get; set; }
        bool IsAccepted { get; }
    }
}