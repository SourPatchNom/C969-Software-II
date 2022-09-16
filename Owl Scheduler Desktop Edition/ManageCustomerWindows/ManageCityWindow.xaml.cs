using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlSchedulerLibrary.OwlSchedule;
using OwlSchedulerLibrary.OwlSchedule.Classes;
using OwlSchedulerLibrary.OwlSchedule.DataModel;

namespace Owl_Scheduler_Desktop_Edition.ManageCustomerWindows
{
    public partial class ManageCityWindow : Window
    {
        private bool _editMode = false;
        
        public ManageCityWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboCityPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Cities;
            ComboBoxCountryPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Countries;
        }

        private void ManageCityWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            _editMode = false;
            RadioEdit.IsChecked = false;
            ComboCityPicker.IsEnabled = false;
            ComboCityPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            ClearAccountInfo();
            Hide();
        }

        private void ClearAccountInfo()
        {
            TextBoxCityName.Text = "";
            ComboBoxCountryPicker.SelectedIndex = -1;
        }
        
        private void RadioNew_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            RadioEdit.IsChecked = false;
            ComboCityPicker.IsEnabled = false;
            ComboCityPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            ClearAccountInfo();
        }

        private void RadioEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ComboCityPicker.HasItems)
            {
                RadioNew.IsChecked = true;
                RadioEdit.IsChecked = false;
                RadioNew_OnClick(sender,e);
                return;
            }
            _editMode = true;
            RadioNew.IsChecked = false;
            DeleteButton.IsEnabled = true;
            ComboCityPicker.IsEnabled = true;
            ComboCityPicker.SelectedIndex = 0;
            DeleteButton.IsEnabled = true;
        }

        private void ComboCityPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboCityPicker.SelectedIndex == -1 || ComboCityPicker == null || !ComboCityPicker.HasItems) return;
            PopulateData();
        }

        private void PopulateData()
        {
            City city = ((City)ComboCityPicker.SelectedItem);
            TextBoxCityName.Text = city.CityName;
            ComboBoxCountryPicker.SelectedItem = OwlScheduler.Instance.CustomerDataModel.Countries.FirstOrDefault(x => x.CountryId == city.Country);
            ValidateInputFields();
        }

        private void ComboBoxCountryPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateInputFields();
        }

        private void ValidateInputFields()
        {
            if (_editMode)
            {
                if (ComboCityPicker.SelectedIndex == -1)
                {
                    LabelErrors.Content = "Select a city to edit.";
                    LabelErrors.Visibility = Visibility.Visible;
                    UpdateSaveButton(false);
                    return;    
                }
            }
            
            if (!OwlSchedulerLibrary.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerCity(TextBoxCityName.Text, out string result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
                
            if (ComboBoxCountryPicker.SelectedIndex == -1)
            { 
                LabelErrors.Content = "Select an associated country.";
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

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var newCity = new City(_editMode ? ((City)ComboCityPicker.SelectedItem).CityId : -1, TextBoxCityName.Text, ((Country)ComboBoxCountryPicker.SelectedItem).CountryId , DateTime.Now, CurrentSession.Instance.CurrentUser.UserName, DateTime.Now,
                CurrentSession.Instance.CurrentUser.UserName);

            if (!CustomerDataModify.SaveCity(newCity, out var result))
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

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            ClearAccountInfo();
            Hide();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_editMode) return;
            if (ComboCityPicker.SelectedIndex == -1) return;
            var confirm = MessageBox.Show("You are about to delete a record! Are you sure?", "Delete City?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;
            if (CustomerDataModify.DeleteCity(((City)ComboCityPicker.SelectedItem).CityId, out string result))
            {
                MessageBox.Show("Record deleted!\n" + result, "Delete City", MessageBoxButton.OK, MessageBoxImage.Information);
                
                if (!ComboCityPicker.HasItems)
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
                ComboCityPicker.SelectedIndex = 0;
                PopulateData();
                return;
            }
            MessageBox.Show(this, "Record delete failed!\n" + result, "Delete City", MessageBoxButton.OK, MessageBoxImage.Error);
            _editMode = false;
            RadioEdit.IsChecked = false;
            RadioNew.IsChecked = true;
            RadioNew_OnClick(sender,e);
            ClearAccountInfo();
            Hide();
        }

        private void TextBoxCityName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputFields();
        }
    }
}