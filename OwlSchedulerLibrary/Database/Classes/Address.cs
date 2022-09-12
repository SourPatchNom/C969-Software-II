using System;

namespace OwlSchedulerLibrary.Classes
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string AddressOne { get; private set; }
        public string AddressTwo { get; private set; }
        public int City { get; private set; }
        public string PostalCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public Address(int addressId, string addressOne, string addressTwo, int city, string postalCode, string phoneNumber, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            AddressId = addressId;
            AddressOne = addressOne;
            AddressTwo = addressTwo;
            City = city;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }
    }
}