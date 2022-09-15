using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using OwlSchedulerLibrary.Classes;
using OwlSchedulerLibrary.Database;
using OwlSchedulerLibrary.Database.Classes;

namespace OwlSchedulerLibrary.OwlSchedule.Classes
{
    public class AppointmentDataModel
    {

        public AppointmentDataModel()
        {
            _selectedWeek = _calendar.GetWeekOfYear(DateTime.UtcNow,CalendarWeekRule.FirstDay,DayOfWeek.Sunday);
        }
        
        private readonly Calendar _calendar = new GregorianCalendar();
        
        private int _selectedMonth = DateTime.UtcNow.Month; //This Month
        private int _selectedYearMonthView = DateTime.UtcNow.Year; //This Year
        private int _selectedYearWeekView = DateTime.UtcNow.Year; //This Year
        private int _selectedWeek;
        
        public readonly BindingList<Appointment>  CurrentUserAppointmentsMaster = new BindingList<Appointment> ();
        public readonly BindingList<Appointment> CurrentUserAppointmentsMonth = new BindingList<Appointment> ();
        public readonly BindingList<Appointment>  CurrentUserAppointmentsWeek = new BindingList<Appointment> ();
        public readonly BindingList<Appointment>  CurrentUserAppointmentsDay = new BindingList<Appointment> ();
        public Appointment CurrentAppointment = null;
        public Appointment CurrentNextAppointment = null;
        
        
        public void UpdateAppointmentData(object sender, PropertyChangedEventArgs e)
        {
            UpdateListMaster();
            UpdateListMonth();
            UpdateListWeek();
            UpdateListDay();
        }

        private void UpdateListMaster()
        {
            CurrentUserAppointmentsMaster.Clear();
            DatabaseHandler.Instance.Appointments.Values.Where(x => x.UserId == CurrentSession.Instance.CurrentUser.UserId).OrderBy(x => x.StartDateTime).ToList().ForEach(x => CurrentUserAppointmentsMaster.Add(x));
        }

        private void UpdateListMonth()
        {
            CurrentUserAppointmentsMonth.Clear();
            DatabaseHandler.Instance.Appointments.Values.Where(x => x.UserId == CurrentSession.Instance.CurrentUser.UserId && x.StartDateTime.Year == _selectedYearMonthView && x.StartDateTime.Month == _selectedMonth).OrderBy(x => x.StartDateTime).ToList().ForEach(x => CurrentUserAppointmentsMonth.Add(x));
        }

        private void UpdateListWeek()
        {
            CurrentUserAppointmentsWeek.Clear();
            DatabaseHandler.Instance.Appointments.Values.Where(x => x.UserId == CurrentSession.Instance.CurrentUser.UserId && x.StartDateTime.Year == _selectedYearWeekView && _calendar.GetWeekOfYear(x.StartDateTime,CalendarWeekRule.FirstDay,DayOfWeek.Sunday) == _selectedWeek).OrderBy(x => x.StartDateTime).ToList().ForEach(x => CurrentUserAppointmentsWeek.Add(x));
        }
        
        private void UpdateListDay()
        {
            CurrentUserAppointmentsDay.Clear();
            DatabaseHandler.Instance.Appointments.Values.Where(x => x.UserId == CurrentSession.Instance.CurrentUser.UserId && x.StartDateTime.Year == DateTime.UtcNow.Year && x.StartDateTime.Day == DateTime.UtcNow.Day && x.EndDateTime > DateTime.Now).OrderBy(x => x.StartDateTime).ToList().ForEach(x => CurrentUserAppointmentsDay.Add(x));
            UpdateNowAndNext();
        }

        public void UpdateNowAndNext()
        {
            if (!CurrentUserAppointmentsDay.Any()) return;
            CurrentAppointment = CurrentUserAppointmentsDay.FirstOrDefault(x => x.StartDateTime < DateTime.Now && x.EndDateTime > DateTime.Now);
            CurrentNextAppointment = CurrentUserAppointmentsDay.FirstOrDefault(x => x.StartDateTime > DateTime.Now);
        }
        
        /// <summary>
        /// For viewing based on selected Month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        public void UpdateMonthView(int month, int year)
        {
            _selectedMonth = month;
            _selectedYearMonthView = year;
            UpdateListMonth();
        }
        
        /// <summary>
        /// For viewing based on selected week.
        /// </summary>
        /// <param name="week"></param>
        /// <param name="year"></param>
        public void UpdateWeekView(int week, int year)
        {
            _selectedWeek = week;
            _selectedYearWeekView = year;
            UpdateListWeek();
        }
    }
}