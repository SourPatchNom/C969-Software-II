using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlSchedule.Classes;

namespace OwlScheduler.Library.OwlSchedule.DataModel
{
    public class UserDataModel
    {
        internal UserDataModel()
        {
            
        }

        public static readonly BindingList<User> AllUsers = new BindingList<User>(); 

        public static void UpdateDataEvent(object sender, PropertyChangedEventArgs e)
        {
            AllUsers.Clear(); 
            DatabaseHandler.Instance.Users.Values.ToList().ForEach(x => AllUsers.Add(x));
        }
    }
}