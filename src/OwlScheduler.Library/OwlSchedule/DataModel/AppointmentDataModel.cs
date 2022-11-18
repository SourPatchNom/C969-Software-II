using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlSchedule.Classes;

namespace OwlScheduler.Library.OwlSchedule.DataModel
{
    /// <summary>
    /// This is the primary data model for appointments.
    /// </summary>
    public class AppointmentDataModel
    {

        internal AppointmentDataModel()
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
        
        public readonly BindingList<Appointment> OtherUserAppointmentsMonth = new BindingList<Appointment> ();
        
        public Appointment CurrentAppointment = null; //Future use?
        public Appointment CurrentNextAppointment = null;
        
        
        /// <summary>
        /// Event handler that updates all appointment data from OwlDatabaseHandler instance..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateAppointmentDataEvent(object sender, PropertyChangedEventArgs e)
        {
            UpdateListMaster();
            UpdateListMonth();
            UpdateListWeek();
            UpdateListDay();
        }

        /// <summary>
        /// Updates the master list of appointments for the current user.
        /// TODO ASSIGNMENT COMMENT This is an exceptional example of the benefits of lambda expressions eliminating lengthy code.
        /// TODO Here we use lambda expressions to select appointments that match the currently logged in user id, order them by start date, and add them to the appointments master BindingList.
        /// TODO I chose lambda expressions over traditional code due to the fact that it reduced multiple lines, additional functions, and potential bug issues. 
        /// </summary>
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
        /// Updates the master list of appointments for the current user.
        /// TODO ASSIGNMENT COMMENT This is an exceptional example of the benefits of lambda expressions eliminating lengthy code.
        /// TODO Here we use lambda expressions to return the sum of appointments of type (string) that match three value equality checks.
        /// TODO I chose lambda expressions over traditional code due to the fact that it reduced multiple lines, additional functions, and potential bug issues. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="comboMonthSelectedItem"></param>
        /// <param name="comboYearSelectedItem"></param>
        /// <returns></returns>
        public static int GetTypeCountByNameGivenMonthYear(string text, int comboMonthSelectedItem, int comboYearSelectedItem)
        {
            return DatabaseHandler.Instance.Appointments.Count(x => x.Value.Type == text && x.Value.StartDateTime.Month == comboMonthSelectedItem && x.Value.StartDateTime.Year == comboYearSelectedItem);
        }
        
        public void UpdateUserReport(int userId, int comboMonthSelectedItem, int comboYearSelectedItem)
        {
            OtherUserAppointmentsMonth.Clear();
            DatabaseHandler.Instance.Appointments.Values.Where(x => x.UserId == userId && x.StartDateTime.Year == comboYearSelectedItem && x.StartDateTime.Month == comboMonthSelectedItem).OrderBy(x => x.StartDateTime).ToList().ForEach(x => OtherUserAppointmentsMonth.Add(x));
        }

        public static int GetLocationCountByNameGivenMonthYear(string text, int comboMonthSelectedItem, int comboYearSelectedItem)
        {
            return DatabaseHandler.Instance.Appointments.Count(x => x.Value.Location == text && x.Value.StartDateTime.Month == comboMonthSelectedItem && x.Value.StartDateTime.Year == comboYearSelectedItem);
        }
    }
}