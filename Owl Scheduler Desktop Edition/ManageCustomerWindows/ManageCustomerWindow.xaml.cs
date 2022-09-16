using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlSchedulerLibrary.OwlSchedule;
using OwlSchedulerLibrary.OwlSchedule.Classes;
using OwlSchedulerLibrary.OwlSchedule.DataModel;
using OwlSchedulerLibrary.OwlSchedule.Helpers;

namespace Owl_Scheduler_Desktop_Edition.ManageCustomerWindows
{
    public partial class ManageCustomerWindow : Window
    {
        private bool _editMode = false;
        private ManageAddressWindow _manageAddressWindow = new ManageAddressWindow();
        private ManageCityWindow _manageCityWindow = new ManageCityWindow();
        private ManageCountryWindow _manageCountryWindow = new ManageCountryWindow();
        
        public ManageCustomerWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;
            ComboAccountPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Customers;
            ComboAddressPicker.ItemsSource = OwlScheduler.Instance.CustomerDataModel.Addresses;
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!CurrentSession.Instance.IsLoggedIn) Hide();
        }

        private void WindowCustomer_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ClearAccountInfo();
            Hide();
        }

        private void ComboAccountPicker_OnNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OwlScheduler.Instance.CustomerDataModel.Customers.Any(x => x == ComboAccountPicker.SelectedItem as Customer))
            {
                PopulateAccountInfo();
                CheckIfFormComplete();
                return;
            }
            ClearAccountInfo();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var newCustomer = GetNewCustomerToSave();

            if (!CustomerDataModify.SaveCustomer(newCustomer, out var result))
            {
                MessageBox.Show(result, "Save Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                ClearAccountInfo();
                Hide();
                return;
            }
            MessageBox.Show("Saved!", "Jobs Done!", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearAccountInfo();
            Hide();
        }

        private Customer GetNewCustomerToSave()
        {

            if (_editMode)
            {
                var oldCustomer = ((Customer)ComboAccountPicker.SelectedItem);
                return new Customer(oldCustomer.CustomerId, TextBoxName.Text, oldCustomer.CustomerAddress, ComboStatusPicker.SelectedIndex == 0, oldCustomer.CreateDateTime, 
                    oldCustomer.CreateBy, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName);
            }

            return new Customer(-1, TextBoxName.Text, ((Address)ComboAddressPicker.SelectedItem).AddressId, ComboStatusPicker.SelectedIndex == 0, 
                DateTime.Now, CurrentSession.Instance.CurrentUser.UserName, DateTime.Now, CurrentSession.Instance.CurrentUser.UserName);
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearAccountInfo();
            Hide();
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_editMode) return;
            if (ComboAccountPicker.SelectedIndex == -1) return;
            var confirm = MessageBox.Show("You are about to delete a record! Are you sure?", "Delete Appointment?", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;
            if (CustomerDataModify.DeleteCustomer(((Customer)ComboAccountPicker.SelectedItem).CustomerId, out string result))
            {
                MessageBox.Show("Record deleted!\n" + result, "Delete Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                
                if (!ComboAccountPicker.HasItems)
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
                ComboAccountPicker.SelectedIndex = 0;
                PopulateAccountInfo();
                return;
            }
            MessageBox.Show(this, "Record delete failed!\n" + result, "Delete Customer", MessageBoxButton.OK, MessageBoxImage.Error);
            _editMode = false;
            RadioEdit.IsChecked = false;
            RadioNew.IsChecked = true;
            RadioNew_OnClick(sender,e);
            ClearAccountInfo();
            Hide();
        }

        private void TextBoxName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void PopulateAccountInfo()
        {
            var customer = (Customer)ComboAccountPicker.SelectedItem;
            TextBoxName.Text = customer.CustomerName;
            ComboStatusPicker.SelectedIndex = customer.Active ? 0 : 1;
            ComboAddressPicker.SelectedItem = OwlScheduler.Instance.CustomerDataModel.Addresses.First(x => x.AddressId == customer.CustomerAddress); 
            UpdateAddressFields(OwlScheduler.Instance.CustomerDataModel.Addresses.First(x => x.AddressId == customer.CustomerAddress));
        }
        
        private void ClearAccountInfo()
        {
            RadioNew.IsChecked = true;
            RadioEdit.IsChecked = false;
            TextBoxName.Clear();
            ComboAccountPicker.SelectedIndex = -1;
            ComboStatusPicker.SelectedIndex = 0;
            ComboAddressPicker.SelectedIndex = -1;
            TextBoxPhone.Clear();
            TextBoxAddressLineOne.Clear();
            TextBoxAddressLineTwo.Clear();
            TextBoxCity.Clear();
            TextBoxCountry.Clear();
            TextBoxPostalLine.Clear();
        }

        private void CheckIfFormComplete()
        {
            if (!CustomerDataFormatCheck.CorrectFormatCustomerName(TextBoxName.Text, out var result))
            {
                UpdateSaveButtonStatus(false, result);
                return;
            }

            if (ComboAddressPicker.SelectedIndex == -1)
            {
                UpdateSaveButtonStatus(false, "Select an address!");
                return;
            }
            
            UpdateSaveButtonStatus(true, result);
        }
        
        private void UpdateSaveButtonStatus(bool canSave, string erText)
        {
            if (canSave)
            {
                LabelErrors.Visibility = Visibility.Hidden;
                LabelErrors.Content = "";
                SaveButton.IsEnabled = true;
                return;    
            }
            LabelErrors.Visibility = Visibility.Visible;
            LabelErrors.Content = erText;
            SaveButton.IsEnabled = false;
        }

        private void ComboAddressPicker_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAddressFields((Address)ComboAddressPicker.SelectedItem);
            CheckIfFormComplete();
        }

        private void UpdateAddressFields(Address address)
        {
            if (address == null) return;
            var city = OwlScheduler.Instance.CustomerDataModel.Cities.First(x => x.CityId == address.CityId);
            var country = OwlScheduler.Instance.CustomerDataModel.Countries.First(x => x.CountryId == city.Country);
            TextBoxPhone.Text = address.PhoneNumber;
            TextBoxAddressLineOne.Text = address.AddressOne;
            TextBoxAddressLineTwo.Text = address.AddressTwo;
            TextBoxPostalLine.Text = address.PostalCode;
            TextBoxCity.Text = city.CityName;
            TextBoxCountry.Text = country.CountryName;
        }
        

        private void RadioNew_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            RadioEdit.IsChecked = false;
            ComboAccountPicker.IsEnabled = false;
            ComboAccountPicker.SelectedIndex = -1;
            DeleteButton.IsEnabled = false;
            ClearAccountInfo();
        }

        private void RadioEdit_OnClick(object sender, RoutedEventArgs e)
        {
            _editMode = true;
            RadioNew.IsChecked = false;
            DeleteButton.IsEnabled = true;
            ComboAccountPicker.IsEnabled = true;
            ComboAccountPicker.SelectedIndex = 0;
            DeleteButton.IsEnabled = true;
        }

        private void ButtonManageAddresses_OnClick(object sender, RoutedEventArgs e)
        {
            _manageAddressWindow.Show();
            _manageAddressWindow.Activate();
        }

        private void ButtonManageCity_OnClick(object sender, RoutedEventArgs e)
        {
            _manageCityWindow.Show();
            _manageCityWindow.Activate();
        }

        private void ButtonManageCountry_OnClick(object sender, RoutedEventArgs e)
        {
            _manageCountryWindow.Show();
            _manageCountryWindow.Activate();
        }
    }
}