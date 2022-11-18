using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlScheduler.Library.OwlSchedule;
using OwlScheduler.Library.OwlSchedule.Classes;
using OwlScheduler.Library.OwlSchedule.DataModel;

namespace OwlScheduler.DesktopEdition.ManageCustomerWindows
{
    public partial class ManageAddressWindow : Window
    {
        private bool _editMode = false;
        
        public ManageAddressWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboAddressPicker.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.CustomerDataModel.Addresses;
            ComboAddressPicker.MaxDropDownHeight = 200;
            ComboBoxCityPicker.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.CustomerDataModel.Cities;
            ComboBoxCityPicker.MaxDropDownHeight = 200;
        }

        private void ManageAddressWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            RadioEdit.IsChecked = false;
            ComboAddressPicker.IsEnabled = false;
            ComboAddressPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            ClearAccountInfo();
            Hide();
        }

        private void ClearAccountInfo()
        {
            _editMode = false;
            ComboAddressPicker.IsEnabled = false;
            ComboAddressPicker.SelectedIndex = -1;
            TextBoxPhone.Clear();
            TextBoxAddressLineOne.Clear();
            TextBoxAddressLineTwo.Clear();
            TextBoxPostalLine.Clear();
            ComboBoxCityPicker.SelectedIndex = -1;
            TextBoxCountry.Clear();
        }
        
        private void ValidateInputData()
        {
            if (_editMode)
            {
                if (ComboAddressPicker.SelectedIndex == -1)
                {
                    LabelErrors.Content = "Select an address to edit.";
                    LabelErrors.Visibility = Visibility.Visible;
                    UpdateSaveButton(false);
                    return;    
                }   
            }
            
            if (!Library.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerPhone(TextBoxPhone.Text, out string result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
            
            if (!Library.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerAddressOne(TextBoxAddressLineOne.Text, out result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
            
            if (!Library.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerAddressTwo(TextBoxAddressLineTwo.Text, out result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
            
            if (!Library.OwlSchedule.Helpers.CustomerDataFormatCheck.CorrectFormatCustomerPostal(TextBoxPostalLine.Text, out result))
            {
                LabelErrors.Content = result;
                LabelErrors.Visibility = Visibility.Visible;
                UpdateSaveButton(false);
                return;
            }
            
            if (ComboBoxCityPicker.SelectedIndex == -1)
            {
                LabelErrors.Content = "Select an associated city!";
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
        
        private void PopulateData()
        {
            if (!_editMode) return;
            var address = ((Address)ComboAddressPicker.SelectedItem);
            TextBoxPhone.Text = address.PhoneNumber;
            TextBoxAddressLineOne.Text = address.AddressOne;
            TextBoxAddressLineTwo.Text = address.AddressTwo;
            TextBoxPostalLine.Text = address.PostalCode;
            var city = Library.OwlSchedule.OwlScheduler.Instance.CustomerDataModel.Cities.FirstOrDefault(x => x.CityId == address.CityId);
            if (city == null) throw new Exception("No associated city in records! Data error!");
            ComboBoxCityPicker.SelectedItem = city;
        }

        private void RadioNew_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            RadioEdit.IsChecked = false;
            ComboAddressPicker.IsEnabled = false;
            ComboAddressPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            ClearAccountInfo();
            ValidateInputData();
        }

        private void RadioEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ComboAddressPicker.HasItems)
            {
                RadioNew.IsChecked = true;
                RadioEdit.IsChecked = false;
                RadioNew_OnClick(sender,e);
                return;
            }
            _editMode = true;
            RadioNew.IsChecked = false;
            DeleteButton.IsEnabled = true;
            ComboAddressPicker.IsEnabled = true;
            ComboAddressPicker.SelectedIndex = 0;
            DeleteButton.IsEnabled = true;
            PopulateData();
            ValidateInputData();
        }

        private void ComboAddressPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboAddressPicker.SelectedIndex == -1 || !_editMode || !ComboAddressPicker.HasItems || ComboAddressPicker == null) return;
            PopulateData();
            ValidateInputData();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var newAddress = new Address(_editMode ? ((Address)ComboAddressPicker.SelectedItem).AddressId : -1, TextBoxAddressLineOne.Text, TextBoxAddressLineTwo.Text, 
                ((City)ComboBoxCityPicker.SelectedItem).CityId, TextBoxPostalLine.Text, TextBoxPhone.Text, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName, DateTime.Now,
                CurrentSession.Instance.CurrentUser.UserName);

            if (!CustomerDataModify.SaveAddress(newAddress, out var result))
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
            if (ComboAddressPicker.SelectedIndex == -1) return;
            var confirm = MessageBox.Show("You are about to delete a record! Are you sure?", "Delete City?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;
            if (CustomerDataModify.DeleteAddress(((Address)ComboAddressPicker.SelectedItem).AddressId, out string result))
            {
                MessageBox.Show("Record deleted!\n" + result, "Delete City", MessageBoxButton.OK, MessageBoxImage.Information);
                
                if (!ComboAddressPicker.HasItems)
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
                ComboAddressPicker.SelectedIndex = 0;
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

        private void ComboBoxCityPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxCityPicker == null || ComboBoxCityPicker.SelectedIndex == -1) return;
            if (!ComboBoxCityPicker.HasItems)
            {
                ComboBoxCityPicker.SelectedIndex = -1;
                TextBoxCountry.Text = "";
                return;
            }
            
            var countryName = Library.OwlSchedule.OwlScheduler.Instance.CustomerDataModel.Countries.FirstOrDefault(x => x.CountryId == ((City)ComboBoxCityPicker.SelectedItem).Country)?.CountryName;
            if (countryName != null) TextBoxCountry.Text = countryName;
            ValidateInputData();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputData();
        }
    }
}