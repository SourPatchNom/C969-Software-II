using System;
using System.ComponentModel;
using OwlSchedulerLibrary.Database;
using OwlSchedulerLibrary.OwlLogger;
using OwlSchedulerLibrary.OwlSchedule.Classes;

namespace OwlSchedulerLibrary.OwlSchedule
{
    public sealed class OwlScheduler
    {
        private static readonly Lazy<OwlScheduler> LazySingleton = new Lazy<OwlScheduler>(() => new OwlScheduler());

        public static OwlScheduler Instance => LazySingleton.Value;
        
        public AppointmentDataModel AppointmentDataModel { get; private set; } = new AppointmentDataModel();
        public CustomerDataModel CustomerDataModel { get; private set; } = new CustomerDataModel();

        public static readonly int BusinessHourAmOpen = 8;
        public static readonly int BusinessHourPmClose = 16;
        
        private OwlScheduler()
        {
            DatabaseHandler.Instance.DatabaseInformationUpdated += AppointmentDataModel.UpdateAppointmentData;
            DatabaseHandler.Instance.DatabaseInformationUpdated += CustomerDataModel.UpdateData;
        }

        public void Initialize()
        {
            LogHandler.LogMessage("OwlScheduler", "Initializing.");
            CurrentSession.Instance.PropertyChanged += HandleLogin;

            LogHandler.LogMessage("OwlScheduler", "Initialized, waiting for user login.");
        }

        private void HandleLogin(object o,PropertyChangedEventArgs e)
        {
            if (CurrentSession.Instance.IsLoggedIn)
            {
                LogHandler.LogMessage("OwlScheduler", "User login updated, attempting to access database!");
                DatabaseInitialize();
                return;
            }

            ClearDatabaseInformation();
        }

        
        /// <summary>
        /// Intended to clear the class data of all information for a logout.
        /// </summary>
        private void ClearDatabaseInformation()
        {
            DatabaseHandler.Instance.ClearLocalData();
        }

        /// <summary>
        /// Populates schedule information from the database.
        /// </summary>
        private void DatabaseInitialize()
        {
            DatabaseHandler.Instance.SyncFromDatabase();
        }
    }
}