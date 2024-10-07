namespace Caliburn.Micro.HelloScreens.Shell
{
    using Framework;

    public class BusyIndicatorViewModel : Screen, IBusyIndicator
    {
        private string _busyMessage = "Busy";

        public string BusyMessage
        {
            get => _busyMessage;
            set
            {
                if (value != _busyMessage)
                {
                    _busyMessage = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        // TODO: this logic will go int he Dialog Conductor

        public async Task SetBusyAsync(string busyMessage)
        {
            await Execute.OnUIThreadAsync(() => Task.Run(() =>
            {
                BusyMessage = busyMessage;
            }));
        }

        public void ClearBusy()
        {
            Execute.OnUIThread(() =>
            {
                BusyMessage = "Busy...";
            });
        }

    }
}
