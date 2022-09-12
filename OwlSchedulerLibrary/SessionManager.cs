using System;
using System.ComponentModel;
using OwlSchedulerLibrary.Classes;
using OwlSchedulerLibrary.Database;

namespace OwlSchedulerLibrary
{
    public sealed class SessionManager
    {
        private static readonly Lazy<SessionManager> LazySingleton = new Lazy<SessionManager>(() => new SessionManager());

        public static SessionManager Instance => LazySingleton.Value;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoggedIn { get; private set; } = false;

        public string CurrentUser { get; private set; }

        public void Logout()
        {
            IsLoggedIn = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User Logged Out!"));
            LogHandler.Instance.LogMessage("User Authentication", "User " + CurrentUser + " logged out.");
        }

        public bool ProcessLoginAttempt(string username, string password)
        {
            if (!AuthenticateUser(username, password)) return false;
            IsLoggedIn = true;
            CurrentUser = username;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User " + CurrentUser + " logged In!"));
            return true;
        }

        private static bool AuthenticateUser(string u, string p)
        {
            var isGood = DatabaseHandler.Instance.LoginUser(u, p);
            LogHandler.Instance.LogMessage("User Authentication", isGood ? "User " + u + " successfully logged in!" : "Login Attempt Failed! Incorrect credentials given by " + u);
            return isGood;
        }
    }
}