using System;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;

namespace OwlSchedulerLibrary
{
    public class OwlScheduler
    {
        private static readonly Lazy<OwlScheduler> LazySingleton = new Lazy<OwlScheduler>(() => new OwlScheduler());

        public static OwlScheduler Instance => LazySingleton.Value;

        private OwlScheduler()
        {
            
        }

        public void Initialize()
        {
            LogHandler.Instance.LogMessage("OwlScheduler", "Initializing.");
            SessionManager.Instance.PropertyChanged += HandleLogin;

            LogHandler.Instance.LogMessage("OwlScheduler", "Initialized, waiting for user login.");
        }

        private void HandleLogin(object o,PropertyChangedEventArgs e)
        {
            if (SessionManager.Instance.IsLoggedIn)
            {
                LogHandler.Instance.LogMessage("OwlScheduler", "User login updated, attempting to access database!");
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
            
        }

        /// <summary>
        /// Populates schedule information from the database.
        /// </summary>
        private void DatabaseInitialize()
        {
            
        }
    }
}