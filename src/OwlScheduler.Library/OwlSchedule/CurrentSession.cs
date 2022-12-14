using System;
using System.ComponentModel;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlLogger;
using OwlScheduler.Library.OwlSchedule.Classes;

namespace OwlScheduler.Library.OwlSchedule
{
    public sealed class CurrentSession
    {
        private static readonly Lazy<CurrentSession> LazySingleton = new Lazy<CurrentSession>(() => new CurrentSession());

        public static CurrentSession Instance => LazySingleton.Value;

        private CurrentSession()
        {
        
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoggedIn { get; private set; }

        public User CurrentUser { get; private set; }

        public void Logout()
        {
            IsLoggedIn = false;
            CurrentUser = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User Logged Out!"));
            LogHandler.LogMessage("User Authentication", "User " + CurrentUser?.UserName + " logged out.");
        }

        public bool ProcessLoginAttempt(string username, string password)
        {
            if (!AuthenticateUser(username, password)) return false;
            IsLoggedIn = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User " + CurrentUser?.UserName + " logged In!"));
            return true;
        }

        private bool AuthenticateUser(string u, string p)
        {
            var isGood = DatabaseHandler.Instance.LoginUser(u, p, out User user);
            if (isGood) CurrentUser = user;
            LogHandler.LogMessage("User Authentication ####### ", isGood ? "User " + u + " successfully logged in!" : "Login Attempt Failed! Incorrect credentials given by " + u);
            return isGood;
        }
    }
}