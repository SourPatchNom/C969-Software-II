using System;
using System.ComponentModel;

namespace OwlSchedulerLibrary
{
    public sealed class SessionManager
    {
        private static readonly Lazy<SessionManager> LazySingleton = new Lazy<SessionManager>(() => new SessionManager());

        public static SessionManager Instance => LazySingleton.Value;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private string _currentUser;
        
        private bool _isLoggedIn = false;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
        }

        public void Logout()
        {
            _isLoggedIn = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User Logged Out!"));
            LogHandler.Instance.LogMessage("User Authentication", "User " + _currentUser + " logged out.");
        }

        public bool ProcessLoginAttempt(string username, string password)
        {
            if (AuthenticateUser(username,password))
            {
                _isLoggedIn = true;
                _currentUser = username;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("User " + _currentUser + " logged In!"));
                return true;
            }

            return false;
        }

        private bool AuthenticateUser(string u, string p)
        {
            bool isGood = u == "test" && p == "test";
            LogHandler.Instance.LogMessage("User Authentication", isGood ? "User " + u + " successfully logged in!": "Login Attempt Failed! Incorrect credentials given by " + u);
            return isGood;
        }
    }
}