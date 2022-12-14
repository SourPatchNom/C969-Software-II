using System;
using System.ComponentModel;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlLogger;
using OwlScheduler.Library.OwlSchedule.DataModel;

namespace OwlScheduler.Library.OwlSchedule
{
    public sealed class OwlScheduler
    {
        private static readonly Lazy<OwlScheduler> LazySingleton = new Lazy<OwlScheduler>(() => new OwlScheduler());

        public static OwlScheduler Instance => LazySingleton.Value;
        
        public AppointmentDataModel AppointmentDataModel { get; private set; } = new AppointmentDataModel();
        public CustomerDataModel CustomerDataModel { get; private set; } = new CustomerDataModel();
        public UserDataModel UserDataModel { get; private set; } = new UserDataModel();

        public const int BusinessHourOpen = 8;
        public const int BusinessHourClose = 17;

        private OwlScheduler()
        {
            DatabaseHandler.Instance.DatabaseInformationUpdated += AppointmentDataModel.UpdateAppointmentDataEvent;
            DatabaseHandler.Instance.DatabaseInformationUpdated += CustomerDataModel.UpdateDataEvent;
            DatabaseHandler.Instance.DatabaseInformationUpdated += UserDataModel.UpdateDataEvent;
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