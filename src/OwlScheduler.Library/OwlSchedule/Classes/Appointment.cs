using System;

namespace OwlScheduler.Library.OwlSchedule.Classes
{
    public class Appointment
    {
        public int AppointmentId { get; private set; }
        public int CustomerId { get; private set; }
        public int UserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public string Contact { get; private set; }
        public string Type { get; private set; }
        public string Url { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public DateTime CreateDateTime { get; private set; }
        public string CreateBy { get; private set; }
        public DateTime LastUpdateDateTime { get; private set; }
        public string LastUpdateBy { get; private set; }

        public Appointment(int appointmentId, int customerId, int userId, string title, string description, string location, string contact, string type, string url, DateTime startDateTime, DateTime endDateTime, DateTime createDateTime, string createBy, DateTime lastUpdateDateTime, string lastUpdateBy)
        {
            AppointmentId = appointmentId;
            CustomerId = customerId;
            UserId = userId;
            Title = title;
            Description = description;
            Location = location;
            Contact = contact;
            Type = type;
            Url = url;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            CreateDateTime = createDateTime;
            CreateBy = createBy;
            LastUpdateDateTime = lastUpdateDateTime;
            LastUpdateBy = lastUpdateBy;
        }
        
    }
}