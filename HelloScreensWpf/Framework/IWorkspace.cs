namespace Caliburn.Micro.HelloScreens.Framework
{
    using JetBrains.Annotations;

    public interface IWorkspace
    {
        [UsedImplicitly]
        string Icon { get; }
        [UsedImplicitly]
        string IconName { get; }
        [UsedImplicitly]
        string Status { get; }
        [UsedImplicitly]
        int DisplayOrder { get; }

        [UsedImplicitly]
        void Show();
    }
}