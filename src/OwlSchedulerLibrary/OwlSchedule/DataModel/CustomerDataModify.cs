using System;
using OwlSchedulerLibrary.OwlDatabase;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlSchedule.DataModel
{
    public static class CustomerDataModify
    {

        public static bool SaveCustomer(Customer newCustomer, out string result)
        {
            try
            {
                if (newCustomer.CustomerId == -1)
                {
                    if (DatabaseHandler.Instance.InsertCustomer(newCustomer) != -1)
                    {
                        result = "Success!";
                        return true;
                    }
                    result = "Failed to insert new customer!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateCustomer(newCustomer) != -1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to update customer!";
                return false;
                
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool SaveAddress(Address newAddress, out string result)
        {
            try
            {
                if (newAddress.AddressId == -1)
                {
                    if (DatabaseHandler.Instance.InsertAddress(newAddress) != -1)
                    {
                        result = "Success!";
                        return true;
                    }
                    result = "Failed to insert new address!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateAddress(newAddress) != -1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to update address!";
                return false;
                
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool SaveCity(City newCity, out string result)
        {
            try
            {
                if (newCity.CityId == -1)
                {
                    if (DatabaseHandler.Instance.InsertCity(newCity) != -1)
                    {
                        result = "Success!";
                        return true;
                    }
                    result = "Failed to insert new city!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateCity(newCity) != -1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to update city!";
                return false;
                
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool SaveCountry(Country newCountry, out string result)
        {
            try
            {
                if (newCountry.CountryId == -1)
                {
                    if (DatabaseHandler.Instance.InsertCountry(newCountry) != -1)
                    {
                        result = "Success!";
                        return true;
                    }
                    result = "Failed to insert new country!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateCountry(newCountry) != -1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to update country!";
                return false;
                
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }

        public static bool DeleteCustomer(int id, out string result)
        {
            try
            {
                if (DatabaseHandler.Instance.CustomerInAppointments(id))
                {
                    result = "Customer cannot be deleted, they have appointments scheduled!";
                    return false;
                }
                
                if (DatabaseHandler.Instance.DeleteCustomer(id) == 1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to delete customer!";
                return false;   
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool DeleteAddress(int id, out string result)
        {
            try
            {
                if (DatabaseHandler.Instance.AddressInCustomer(id))
                {
                    result = "Address cannot be deleted, they have customers associated!";
                    return false;
                }
                
                if (DatabaseHandler.Instance.DeleteAddress(id) == 1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to delete address!";
                return false;   
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool DeleteCity(int id, out string result)
        {
            try
            {
                if (DatabaseHandler.Instance.CityInAddress(id))
                {
                    result = "City cannot be deleted, they have address associated!";
                    return false;
                }
                
                if (DatabaseHandler.Instance.DeleteCity(id) == 1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to delete city!";
                return false;   
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
        
        public static bool DeleteCountry(int id, out string result)
        {
            try
            {
                if (DatabaseHandler.Instance.CountryInCity(id))
                {
                    result = "Country cannot be deleted, they have city associated!";
                    return false;
                }
                
                if (DatabaseHandler.Instance.DeleteCountry(id) == 1)
                {
                    result = "Success!";
                    return true;
                }
                result = "Failed to delete country!";
                return false;   
            }
            catch (Exception e)
            {
                result = e.Message;
                throw;
            }
        }
    }
}