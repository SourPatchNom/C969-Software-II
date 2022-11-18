using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using OwlScheduler.Library.OwlSchedule;
using OwlScheduler.Library.OwlSchedule.Classes;
using OwlScheduler.Library.OwlSchedule.DataModel;

namespace OwlScheduler.DesktopEdition.ManageCustomerWindows
{
    public partial class ManageCountryWindow : Window
    {
        private bool _editMode = false;
        
        public ManageCountryWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboCountryPicker.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.CustomerDataModel.Countries;
            ComboCountryPicker.MaxDropDownHeight = 200;
        }

        private void RadioNew_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            RadioEdit.IsChecked = false;
            ComboCountryPicker.IsEnabled = false;
            ComboCountryPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            TextBoxCountryName.Text = "";
        }

        private void RadioEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ComboCountryPicker.HasItems)
            {
                RadioNew.IsChecked = true;
                RadioEdit.IsChecked = false;
                RadioNew_OnClick(sender,e);
                return;
            }
            _editMode = true;
            RadioNew.IsChecked = false;
            DeleteButton.IsEnabled = true;
            ComboCountryPicker.IsEnabled = true;
            ComboCountryPicker.SelectedIndex = 0;
            DeleteButton.IsEnabled = true;
        }

        private void ComboCountryPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboCountryPicker == null || !ComboCountryPicker.HasItems || ComboCountryPicker.SelectedIndex == -1) return;
            TextBoxCountryName.Text = ((Country)ComboCountryPicker.SelectedItem).CountryName;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var newCountry = new Country(_editMode ? ((Country)ComboCountryPicker.SelectedItem).CountryId : -1, TextBoxCountryName.Text, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName, DateTime.Now,
                CurrentSession.Instance.CurrentUser.UserName);

            if (!CustomerDataModify.SaveCountry(newCountry, out var result))
            {
                MessageBox.Show(result, "Save Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                RadioEdit.IsChecked = false;
                RadioNew.IsChecked = true;
                RadioNew_OnClick(sender,e);
                ClearAccountInfo();
                Hide();
                return;
            }
            MessageBox.Show("Saved!", "Jobs Done!", MessageBoxButton.OK, MessageBoxImage.Information);
            RadioEdit.IsChecked = false;
            RadioNew.IsChecked = true;
            RadioNew_OnClick(sender,e);
            ClearAccountInfo();
            Hide();
        }

        private void ClearAccountInfo()
        {
            ComboCountryPicker.SelectedIndex = -1;
            TextBoxCountryName.Clear();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            ClearAccountInfo();
            Hide();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_editMode) return;
            if (ComboCountryPicker.SelectedIndex == -1) return;
            var confirm = MessageBox.Show("You are about to delete a record! Are you sure?", "Delete Country?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;
            if (CustomerDataModify.DeleteCountry(((Country)ComboCountryPicker.SelectedItem).CountryId, out string result))
            {
                MessageBox.Show("Record deleted!\n" + result, "Delete Country", MessageBoxButton.OK, MessageBoxImage.Information);
                
                if (!ComboCountryPicker.HasItems)
                {
                    _editMode = false;
                    RadioEdit.IsChecked = false;
                    RadioNew.IsChecked = true;
                    RadioNew_OnClick(sender,e);
                    ClearAccountInfo();
                    return;
                }
                RadioEdit.IsChecked = true;
                RadioNew.IsChecked = false;
                ComboCountryPicker.SelectedIndex = 0;
                TextBoxCountryName.Text = ((Country)ComboCountryPicker.SelectedItem).CountryName;
                return;
            }
            MessageBox.Show(this, "Record delete failed!\n" + result, "Delete Country", MessageBoxButton.OK, MessageBoxImage.Error);
            _editMode = false;
            RadioEdit.IsChecked = false;
            RadioNew.IsChecked = true;
            RadioNew_OnClick(sender,e);
            ClearAccountInfo();
            Hide();
        }

        private void TextBoxCountryName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Library.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerCountry(TextBoxCountryName.Text, out string result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
            LabelErrors.Content = "";
            LabelErrors.Visibility = Visibility.Hidden;
            UpdateSaveButton(true);
        }

        private void UpdateSaveButton(bool canSave)
        {
            SaveButton.IsEnabled = canSave;
        }

        private void ManageCountryWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            _editMode = false;
            RadioNew.IsChecked = true;
            ClearAccountInfo();
            Hide();
        }
    }
}