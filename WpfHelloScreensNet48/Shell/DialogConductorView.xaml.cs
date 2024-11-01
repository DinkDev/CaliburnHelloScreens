namespace Caliburn.Micro.HelloScreens.Shell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    // ReSharper disable once RedundantExtendsListEntry
    public partial class DialogConductorView : UserControl
    {
        private bool _disabled;

        public DialogConductorView()
        {
            InitializeComponent();
            ActiveItem.ContentChanged += OnTransitionCompleted;
            Loaded += OnLoad;
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            if (_disabled)
            {
                DisableBackground();
            }
        }

        void OnTransitionCompleted(object sender, EventArgs e)
        {
            if (ActiveItem.Content == null)
            {
                EnableBackground();
            }
            else
            {
                DisableBackground();

                if (ActiveItem.Content is Control control)
                {
                    control.Focus();
                }
            }
        }

        public void EnableBackground()
        {
            _disabled = false;
            ChangeEnabledState(GetBackground(), true);
        }

        public void DisableBackground()
        {
            _disabled = true;
            ChangeEnabledState(GetBackground(), false);
        }

        IEnumerable<UIElement> GetBackground()
        {
            var contentControl = (ContentControl)Parent;
            var container = (Panel)contentControl.Parent;
            return container.Children.Cast<FrameworkElement>().Where(child => child != contentControl);
        }

        void ChangeEnabledState(IEnumerable<UIElement> background, bool state)
        {
            foreach (var uiElement in background)
            {
                if (uiElement is FrameworkElement control)
                {
                    control.IsEnabled = state;
                }
                //else
                //{
                //    var panel = uiElement as Panel;
                //    if (panel != null)
                //    {
                //        foreach (Control child in panel.Children)
                //        {
                //            ChangeEnabledState(new[] { child }, state);
                //        }
                //    }
                //}
            }
        }
    }
}