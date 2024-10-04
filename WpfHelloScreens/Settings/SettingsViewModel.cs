namespace Caliburn.Micro.HelloScreens.Settings
{
    using System.ComponentModel.Composition;
    using Framework;
    using Nito.AsyncEx.Synchronous;

    [Export(typeof(IWorkspace))]
    public class SettingsViewModel : Screen, IWorkspace
    {
        public SettingsViewModel()
        {
            DisplayName = IconName;
        }

        public string Icon
        {
            get { return "../Settings/Resources/Images/report48.png"; }
        }

        public string IconName
        {
            get { return "Settings"; }
        }

        public string Status
        {
            get { return string.Empty; }
        }

        public void Show()
        {
            var task = ((IConductor)Parent).ActivateItemAsync(this);
            task.WaitAndUnwrapException();
        }
    }
}