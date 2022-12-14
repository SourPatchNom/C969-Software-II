using System.ComponentModel;
using System.Linq;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlSchedule.Classes;

namespace OwlScheduler.Library.OwlSchedule.DataModel
{
    public class CustomerDataModel
    {
        
        public readonly BindingList<Customer> Customers = new BindingList<Customer>();
        public readonly BindingList<Address> Addresses = new BindingList<Address>();
        public readonly BindingList<City> Cities = new BindingList<City>();
        public readonly BindingList<Country> Countries = new BindingList<Country>();

        internal CustomerDataModel()
        {
            UpdateCountries();
            UpdateCities();
            UpdateAddresses();
            UpdateCustomers();
        }

        public void UpdateDataEvent(object sender, PropertyChangedEventArgs e)
        {
            UpdateCountries();
            UpdateCities();
            UpdateAddresses();
            UpdateCustomers();
        }

        /// <summary>
        /// Updates the master list of appointments for the current user.
        /// TODO ASSIGNMENT COMMENT This is an exceptional example of the benefits of lambda expressions eliminating lengthy code.
        /// TODO Here we use lambda expressions to order countries by their name and add them to the countries master BindingList.
        /// TODO I chose lambda expressions over traditional code due to the fact that it reduced multiple lines, additional functions, and potential bug issues. 
        /// </summary>
        private void UpdateCountries()
        {
            Countries.Clear();
            DatabaseHandler.Instance.Countries.Values.OrderBy(x => x.CountryName).ToList().ForEach(x => Countries.Add(x));
        }

        private void UpdateCities()
        {
            Cities.Clear();
            DatabaseHandler.Instance.Cities.Values.OrderBy(x => x.CityName).ToList().ForEach(x => Cities.Add(x));
        }

        private void UpdateAddresses()
        {
            Addresses.Clear();
            DatabaseHandler.Instance.Addresses.Values.OrderBy(x => x.CityId).ToList().ForEach(x => Addresses.Add(x));
        }
        
        private void UpdateCustomers()
        {
            Customers.Clear();
            DatabaseHandler.Instance.Customers.Values.OrderBy(x => x.CustomerName).ToList().ForEach(x => Customers.Add(x));
        }
    }
}