using System;

namespace OwlScheduler.Library.OwlSchedule.Classes
{
    public class Customer
    {
        public int CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public int CustomerAddress { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public Customer(int customerId, string customerName, int customerAddress, bool active, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            Active = active;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }

        public void UpdateAddress(int newAddressId)
        {
            CustomerAddress = newAddressId;
        }
    }
}