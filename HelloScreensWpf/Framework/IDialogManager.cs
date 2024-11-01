namespace Caliburn.Micro.HelloScreens.Framework
{
    using System.Threading.Tasks;

    public interface IDialogManager
    {
        void ShowDialog(IScreen dialogModel);

        Task<bool> ShowMessageBoxAsync(
            string message,
            string title = null,
            MessageBoxButtonns options = MessageBoxButtons.Ok);
    }
}