using OwlSchedulerLibrary.OwlDatabase;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlSchedule.DataModel
{
    public static class AppointmentDataModify
    {
        public static bool SaveAppointment(Appointment appointment, out string result)
        {
            int recordId = -1;
            if (appointment.AppointmentId == -1)
            {
                recordId = DatabaseHandler.Instance.InsertAppointment(appointment);
                if (recordId == -1)
                {
                    result = "Failed to insert appointment in database!";
                    return false;
                }
                result = "Appointment updated in database!";
                return true;
            }
            
            recordId = DatabaseHandler.Instance.UpdateAppointment(appointment);
            if (recordId == -1)
            {
                result = "Failed to update appointment in database!";
                return false;
            }
            result = "Appointment updated in database!";
            return true;
        }
    }
}