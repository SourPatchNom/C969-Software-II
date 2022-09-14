﻿using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using OwlSchedulerLibrary.Classes;
using OwlSchedulerLibrary.Database;

namespace OwlSchedulerLibrary.OwlSchedule.Classes
{
    public static class CustomerEditorSave
    {

        public static bool SaveCustomer(Customer newCustomer, out string s)
        {
            try
            {
                if (newCustomer.CustomerId == -1)
                {
                    if (DatabaseHandler.Instance.InsertCustomer(newCustomer) != -1)
                    {
                        s = "Success!";
                        return true;
                    }
                    s = "Failed to insert new address!";
                    return false;    
                }
                if (DatabaseHandler.Instance.UpdateCustomer(newCustomer) != -1)
                {
                    s = "Success!";
                    return true;
                }
                s = "Failed to update address!";
                return false;
                
            }
            catch (Exception e)
            {
                s = e.Message;
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