using System;

namespace OwlSchedulerLibrary.OwlSchedule.Classes
{
    public class City
    {
        public int CityId { get; private set; }
        public string CityName { get; private set; }
        public int Country { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public City(int cityId, string cityName, int country, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            CityId = cityId;
            CityName = cityName;
            Country = country;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }
    }
}