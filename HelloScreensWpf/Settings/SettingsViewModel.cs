namespace Caliburn.Micro.HelloScreens.Settings
{
    using Framework;
    using Nito.AsyncEx.Synchronous;

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

        public int DisplayOrder { get; } = 3;

        public void Show()
        {
            var task = ((IConductor)Parent).ActivateItemAsync(this);
            task.WaitAndUnwrapException();
        }
    }
}