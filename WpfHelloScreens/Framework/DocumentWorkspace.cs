namespace Caliburn.Micro.HelloScreens.Framework
{
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Nito.AsyncEx.Synchronous;

    public abstract class DocumentWorkspace<TDocument> : Conductor<TDocument>.Collection.OneActive, IDocumentWorkspace
        where TDocument : class, INotifyPropertyChanged, IDeactivate, IHaveDisplayName
    {
        private DocumentWorkspaceState _state/* = DocumentWorkspaceState.Master*/;

        protected DocumentWorkspace()
        {
            Items.CollectionChanged += delegate { NotifyOfPropertyChange(() => Status); };
            DisplayName = IconName;
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            State = DocumentWorkspaceState.Master;
            return base.OnActivateAsync(cancellationToken);
        }

        public DocumentWorkspaceState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyOfPropertyChange(() => State);
                }
            }
        }

        protected IConductor Conductor => (IConductor)Parent;
        public abstract string IconName { get; }
        public abstract string Icon { get; }

        public string Status => Items.Count > 0 ? Items.Count.ToString() : string.Empty;

        public void Show()
        {
            if (!(Parent is IHaveActiveItem haveActive) || haveActive.ActiveItem != this)
            {
                var task = Conductor.ActivateItemAsync(this);
                task.WaitAndUnwrapException();
            }
            else
            {
                DisplayName = IconName;
                State = DocumentWorkspaceState.Master;
            }
        }

        async Task IDocumentWorkspace.EditAsync(object document)
        {
            await EditAsync((TDocument)document);
        }

        //public void Edit(TDocument child) {
        //    Conductor.ActivateItemAsync(this);
        //    State = DocumentWorkspaceState.Detail;
        //    DisplayName = child.DisplayName;
        //    ActivateItemAsync(child);
        //}

        public async Task EditAsync(TDocument child)
        {
            await Conductor.ActivateItemAsync(this);
            State = DocumentWorkspaceState.Detail;
            DisplayName = child.DisplayName;
            await ActivateItemAsync(child);
        }

        //public override void ActivateItem(TDocument item) {
        //    item.Deactivated += OnItemOnDeactivated;
        //    item.PropertyChanged += OnItemPropertyChanged;
        //
        //    base.ActivateItem(item);
        //}

        public override async Task ActivateItemAsync(TDocument item,
            CancellationToken cancellationToken = new CancellationToken())
        {
            item.Deactivated += OnItemOnDeactivatedAsync;
            item.PropertyChanged += OnItemPropertyChanged;

            await base.ActivateItemAsync(item, cancellationToken);
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DisplayName")
            {
                DisplayName = ((TDocument)sender).DisplayName;
            }
        }

        //void OnItemOnDeactivated(object sender, DeactivationEventArgs e)
        //{
        //    var doc = (TDocument)sender;
        //    if(e.WasClosed) {
        //        DisplayName = IconName;
        //        State = DocumentWorkspaceState.Master;
        //        doc.Deactivated -= OnItemOnDeactivated;
        //        doc.PropertyChanged -= OnItemPropertyChanged;
        //    }
        //}

        private async Task OnItemOnDeactivatedAsync(object sender, DeactivationEventArgs e)
        {
            await Task.Run(() =>
            {
                var doc = (TDocument)sender;
                if (e.WasClosed)
                {
                    DisplayName = IconName;
                    State = DocumentWorkspaceState.Master;
                    doc.Deactivated -= OnItemOnDeactivatedAsync;
                    doc.PropertyChanged -= OnItemPropertyChanged;
                }
            }).ConfigureAwait(false);
        }
    }
}