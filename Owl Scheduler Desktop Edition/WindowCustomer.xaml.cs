using System;
using System.ComponentModel;
using System.Windows;
using OwlSchedulerLibrary.OwlSchedule;

namespace Owl_Scheduler_Desktop_Edition
{
    public partial class WindowCustomer : Window
    {
        public WindowCustomer()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!CurrentSession.Instance.IsLoggedIn) Hide();
        }

        private void WindowCustomer_OnDeactivated(object sender, EventArgs e)
        {
            Hide();
        }
    }
}