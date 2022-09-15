using System;
using System.Linq;

namespace OwlSchedulerLibrary.OwlSchedule.Helpers
{
    public static class AppointmentDataFormatCheck
    {
        public static bool CheckAppointmentDateTime(DateTime start, DateTime end, out string result)
        {
            result = "";

            //Before or after open
            if (start.Hour < OwlScheduler.BusinessHourOpen - 1 || end > end.Date.AddHours(OwlScheduler.BusinessHourClose))
            {
                result = "Outside 8-5 Business Hours!";
                return false;
            }
            
            //Overlap 
            if (OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster.Any(x => x.StartDateTime < end && start < x.EndDateTime))
            {
                result = "An appointment is already booked at that time!";
                return false;
            }
            
            return true;
        }
    }
}