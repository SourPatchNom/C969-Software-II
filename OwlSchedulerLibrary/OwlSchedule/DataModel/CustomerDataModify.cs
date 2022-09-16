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
                    result = "Failed to insert new address!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateCustomer(newCustomer) != -1)
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

        public static bool DeleteCustomer(int id, out string result)
        {
            try
            {
                if (DatabaseHandler.Instance.CustomerHasAppointments(id))
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
    }
}

// result = "";
// var insertResult = InsertAddressWithResult(newAddress, out var record);
// if (insertResult)
// {
//     result += "\nNew Address Add Successful!";
//     newCustomer.UpdateAddress(record);
//     if (InsertCustomer(newCustomer))
//     {
//         result += "\nNew Customer Add Successful!";
//         return true;
//     }
//     result += "\nNew Customer Add Failed!";
//     return false;
// }
// result += "\nNew Address Add Failed!";
// return false;