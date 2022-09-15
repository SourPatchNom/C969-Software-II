using System;
using System.ComponentModel;
using System.Windows;
using OwlSchedulerLibrary.OwlSchedule;

namespace Owl_Scheduler_Desktop_Edition.ManageAppointmentWindows
{
    public partial class ManageAppointmentWindow : Window
    {
        public ManageAppointmentWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ClearFieldInfo();
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;
            ComboAppointmentPicker.ItemsSource = OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster;
            ComboCustomerPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Customers;
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (CurrentSession.Instance.IsLoggedIn) return;
            ClearFieldInfo();
            Hide();
        }

        private void WindowAppointment_OnDeactivated(object sender, EventArgs e)
        {
            ClearFieldInfo();
            Hide();
        }

        private void WindowAppointment_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ClearFieldInfo();
            Hide();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearFieldInfo();
            Hide();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void ClearFieldInfo()
        {
            
        }
    }
}