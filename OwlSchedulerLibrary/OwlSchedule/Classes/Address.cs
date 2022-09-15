using System;

namespace OwlSchedulerLibrary.OwlSchedule.Classes
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string AddressOne { get; private set; }
        public string AddressTwo { get; private set; }
        public int CityId { get; private set; }
        public string PostalCode { get; private set; }
        public string PhoneNumber { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public Address(int addressId, string addressOne, string addressTwo, int cityId, string postalCode, string phoneNumber, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            AddressId = addressId;
            AddressOne = addressOne;
            AddressTwo = addressTwo;
            CityId = cityId;
            PostalCode = postalCode;
            PhoneNumber = phoneNumber;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }
    }
}