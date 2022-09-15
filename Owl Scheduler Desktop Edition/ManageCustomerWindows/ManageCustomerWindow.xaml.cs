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

        private void WindowCustomer_OnDeactivated(object sender, EventArgs e)
        {
            ClearAccountInfo();
            Hide();
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
                PopulateAccountInfo(ComboAccountPicker.SelectedItem as Customer);
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


        private void TextBoxName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CheckIfFormComplete();
        }

        private void PopulateAccountInfo(Customer customer)
        {
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

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
    }
}