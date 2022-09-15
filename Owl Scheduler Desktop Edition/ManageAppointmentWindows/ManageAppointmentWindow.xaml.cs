using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlSchedulerLibrary.OwlSchedule;
using OwlSchedulerLibrary.OwlSchedule.Classes;
using OwlSchedulerLibrary.OwlSchedule.Helpers;

namespace Owl_Scheduler_Desktop_Edition.ManageAppointmentWindows
{
    public partial class ManageAppointmentWindow : Window
    {
        private bool _editMode;
        
        public ManageAppointmentWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ClearFieldInfo();
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;
            ComboAppointmentPicker.ItemsSource = OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster;
            ComboCustomerPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Customers;
            DatePickerStart.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Now - TimeSpan.FromDays(1)));
            ComboBoxStartHour.ItemsSource =  Enumerable.Range(OwlScheduler.BusinessHourOpen, OwlScheduler.BusinessHourClose - OwlScheduler.BusinessHourOpen);
            ComboBoxStartMinute.ItemsSource = Enumerable.Range(0, 60);
            ComboBoxDurationHour.ItemsSource = Enumerable.Range(0, 8);
            ComboBoxDurationMinute.ItemsSource = Enumerable.Range(0, 60);
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
            if (DatePickerStart.SelectedDate == null) return;
            DateTime start = DatePickerStart.SelectedDate.Value.AddHours((int)ComboBoxStartHour.SelectedItem).AddMinutes((int)ComboBoxStartMinute.SelectedItem);
            DateTime end = start.AddHours((int)ComboBoxDurationHour.SelectedItem).AddMinutes((int)ComboBoxDurationMinute.SelectedItem);
            bool saved = OwlSchedulerLibrary.OwlSchedule.DataModel.AppointmentDataModify.SaveAppointment(new Appointment(_editMode ? (((Appointment)ComboAppointmentPicker.SelectedItem).AppointmentId) : -1,
                ((Customer)ComboCustomerPicker.SelectedItem).CustomerId, CurrentSession.Instance.CurrentUser.UserId, TextBoxTitle.Text, TextBoxDescription.Text, TextBoxLocation.Text,
                TextBoxContact.Text, TextBoxType.Text, TextBoxUrl.Text, start, end, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName), out string result);
            MessageBox.Show(saved ? "Saved!" : "Unable to save!\n" + result, "Save Appointment", MessageBoxButton.OK, saved ? MessageBoxImage.Information : MessageBoxImage.Error);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearFieldInfo();
            Hide();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_editMode) return;
        }
        
        private void ClearFieldInfo()
        {
            ComboAppointmentPicker.SelectedIndex = -1;
            ComboCustomerPicker.SelectedIndex = -1;
            TextBoxTitle.Clear();
            TextBoxDescription.Clear();
            TextBoxLocation.Clear();
            TextBoxContact.Clear();
            TextBoxType.Clear();
            TextBoxUrl.Clear();
            DatePickerStart.SelectedDate = null;
            ComboBoxStartHour.SelectedIndex = -1;
            ComboBoxStartMinute.SelectedIndex = -1;
            ComboBoxDurationHour.SelectedIndex = -1;
            ComboBoxDurationMinute.SelectedIndex = -1;
        }

        private void RadioNew_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            ComboAppointmentPicker.IsEnabled = false;
            ComboAppointmentPicker.SelectedIndex = -1;
            RadioEdit.IsChecked = false;
            DeleteButton.IsEnabled = false;
            ClearFieldInfo();
        }

        private void RadioEdit_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = true;
            ComboAppointmentPicker.IsEnabled = true;
            ComboAppointmentPicker.SelectedIndex = 0;
            RadioNew.IsChecked = false;
            DeleteButton.IsEnabled = true;
            ClearFieldInfo();
        }

        private void ComboAppointmentPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void ComboCustomerPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxTitle_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxDescription_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxLocation_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxContact_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxType_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void TextBoxUrl_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void DateBoxStart_OnDateChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            CheckIfFormComplete();
        }

        private void ComboBoxStartHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void ComboBoxStartMinute_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void ComboBoxDurationHour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void ComboBoxDurationMinute_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void CheckIfFormComplete()
        {
            if (_editMode)
            {
                if (ComboAppointmentPicker.SelectedIndex == -1)
                {
                    LabelErrors.Visibility = Visibility.Visible;
                    LabelErrors.Content = "Select an appointment to edit!";
                    ToggleSaveButton(false);
                    return;
                }
            }
            
            if (ComboCustomerPicker.SelectedIndex == -1)
            {
                LabelErrors.Visibility = Visibility.Visible;
                LabelErrors.Content = "Select a customer record to edit!";
                ToggleSaveButton(false);
                return;
            }
            
            if (TextBoxTitle.Text == "" || TextBoxDescription.Text == "" || TextBoxLocation.Text == "" || TextBoxContact.Text == "" || TextBoxContact.Text == "" || TextBoxType.Text == "" || TextBoxUrl.Text == "")
            {
                LabelErrors.Visibility = Visibility.Visible;
                LabelErrors.Content = "No text in required field!";
                ToggleSaveButton(false);
                return;
            }
            
            if (!CheckDateTimeBox(out string timeBoxResult))
            {
                LabelErrors.Visibility = Visibility.Visible;
                LabelErrors.Content = timeBoxResult;
                ToggleSaveButton(false);
                return;
            }
            
            //No errors
            LabelErrors.Visibility = Visibility.Hidden;
            LabelErrors.Content = "";
            ToggleSaveButton(true);
        }

        private void ToggleSaveButton(bool b)
        {
            SaveButton.IsEnabled = b;
        }

        private bool CheckDateTimeBox(out string result)
        {
            result = "";
            if (DatePickerStart.SelectedDate == null)
            {
                result = "No start date selected.";
                return false;
            }

            if (ComboBoxStartHour.SelectedIndex == -1 || ComboBoxStartMinute.SelectedIndex == -1)
            {
                result = "Start time not set.";
                return false;
            }

            if (ComboBoxDurationHour.SelectedIndex == -1 || ComboBoxDurationMinute.SelectedIndex == -1)
            {
                result = "Duration not set.";
                return false;
            }

            if ((int)ComboBoxDurationHour.SelectedItem == 0 && (int)ComboBoxDurationMinute.SelectedItem < 15)
            {
                result = "Duration too short. Meetings must be 15 minutes or more!";
                return false;
            }
            
            DateTime start = DatePickerStart.SelectedDate.Value.AddHours((int)ComboBoxStartHour.SelectedItem).AddMinutes((int)ComboBoxStartMinute.SelectedItem);
            DateTime end = start.AddHours((int)ComboBoxDurationHour.SelectedItem).AddMinutes((int)ComboBoxDurationMinute.SelectedItem);
            if (!AppointmentDataFormatCheck.CheckAppointmentDateTime(start, end, out result))
            {
                return false;
            }
            
            return true;
        }
    }
}