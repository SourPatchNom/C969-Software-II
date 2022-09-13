using System;
using System.ComponentModel;
using System.Windows;
using OwlSchedulerLibrary.OwlSchedule;

namespace Owl_Scheduler_Desktop_Edition
{
    public partial class WindowAppointment : Window
    {
        public WindowAppointment()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CurrentSession.Instance.IsLoggedIn) Hide();
        }

        private void WindowAppointment_OnDeactivated(object sender, EventArgs e)
        {
            Hide();
        }
    }
}