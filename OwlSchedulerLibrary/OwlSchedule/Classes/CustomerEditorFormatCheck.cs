using System;

namespace OwlSchedulerLibrary.OwlSchedule.Classes
{
    public static class CustomerEditorFormatCheck
    {
        public static bool CorrectFormatCustomerName(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Name Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerPhone(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Phone Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerAddressOne(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Address Line One Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerAddressTwo(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Address Line Two Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerCity(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer City Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerCountry(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Country Provided! ";
                return false;
            }
            return true;
        }
        
        public static bool CorrectFormatCustomerPostal(string input, out string result)
        {
            result = "";
            if (input.Length < 1)
            {
                result = "No Customer Postal Provided! ";
                return false;
            }
            return true;
        }
    }
}