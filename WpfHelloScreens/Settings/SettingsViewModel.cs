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
            // ReSharper disable VirtualMemberCallInConstructor
            DisplayName = IconName;
            // ReSharper restore VirtualMemberCallInConstructor
        }

        public string Icon => "../Settings/Resources/Images/report48.png";

        public string IconName => "Settings";

        public string Status => string.Empty;

        public void Show()
        {
            var task = ((IConductor)Parent).ActivateItemAsync(this);
            task.WaitAndUnwrapException();
        }
    }
}