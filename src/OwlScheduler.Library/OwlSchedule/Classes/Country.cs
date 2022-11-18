using System;

namespace OwlScheduler.Library.OwlSchedule.Classes
{
    public class Country
    {
        public int CountryId { get; private set; }
        public string CountryName { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public Country(int countryId, string countryName, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            CountryId = countryId;
            CountryName = countryName;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }
    }
}