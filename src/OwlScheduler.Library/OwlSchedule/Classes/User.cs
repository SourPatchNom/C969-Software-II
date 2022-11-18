namespace OwlScheduler.Library.OwlSchedule.Classes
{
    public class User
    {
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public bool Active { get; private set; }

        public User(int userId, string userName, int active)
        {
            UserId = userId;
            UserName = userName;
            Active = active == 1;
        }
    }
}