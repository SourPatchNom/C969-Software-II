using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlScheduler.Library.OwlSchedule;
using OwlScheduler.Library.OwlSchedule.Classes;
using OwlScheduler.Library.OwlSchedule.DataModel;

namespace OwlScheduler.DesktopEdition.ReportingWindows
{
    public partial class ConsultantScheduleWindow : Window
    {
        public ConsultantScheduleWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboBoxUser.ItemsSource = UserDataModel.AllUsers;
            ComboMonth.ItemsSource = Enumerable.Range(1,12);
            ComboMonth.SelectedItem = DateTime.Now.Month;
            ComboYear.ItemsSource = Enumerable.Range(DateTime.Now.Year-10,20);
            ComboYear.SelectedItem = DateTime.Now.Year;
            DataGridUserSchedule.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.OtherUserAppointmentsMonth;
        }


        
        
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }
        
        private void Calculate()
        {
            if (ComboBoxUser.SelectedIndex == -1 || !ComboBoxUser.HasItems)
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Select a user!";
                return;
            }
            
            if (ComboMonth.SelectedIndex == -1)
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Pick a month!";
                return;
            }
            
            if (ComboYear.SelectedIndex == -1)
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Pick a year!";
                return;
            }
            
            LabelError.Visibility = Visibility.Hidden;
            LabelError.Content = "";
            Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.UpdateUserReport(((User)ComboBoxUser.SelectedItem).UserId,(int)ComboMonth.SelectedItem,(int)ComboYear.SelectedItem);
        }

        private void ConsultantScheduleWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}