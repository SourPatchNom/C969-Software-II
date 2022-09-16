using System.Linq;

namespace OwlSchedulerLibrary.OwlSchedule.Helpers
{
    public static class CustomerDataFormatCheck
    {
        public static bool CorrectFormatCustomerName(string input, out string result)
        {
            result = "";
            
            //Has value
            if (input.Length < 1)
            {
                result = "No Customer Name Provided! ";
                return false;
            }
            
            //Letters and spaces only
            if (!input.All(x => char.IsLetter(x) || x == ' '))
            {
                result = "Only letters and spaces allowed in name! ";
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
            if (!input.All(x => char.IsNumber(x) || x == '-'))
            {
                result = "Numbers and - only for phone please! ";
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
            
            if (!input.All(x => char.IsLetter(x) || char.IsNumber(x) || x == ' '))
            {
                result = "Only letters, numbers, and spaces allowed in name! ";
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
            
            if (!input.All(x => char.IsLetter(x) || char.IsNumber(x) || x == ' '))
            {
                result = "Only letters, numbers, and spaces allowed in name! ";
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