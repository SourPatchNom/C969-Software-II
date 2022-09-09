using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using OwlSchedulerLibrary;

namespace Owl_Scheduler_Desktop_Edition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private WindowUserLogin _windowUserLogin = new WindowUserLogin();
        
        public MainWindow()
        {
            InitializeComponent();
            LogHandler.Instance.LogMessage("Main Window", "Initialized.");
            OwlScheduler.Instance.Initialize();

            
            // Hide();
            // _windowUserLogin.Show();
            // _windowUserLogin.Activate();
            
            SessionManager.Instance.PropertyChanged += InstanceOnPropertyChanged;
            
            SessionManager.Instance.ProcessLoginAttempt("test", "test");
        }

        private void InstanceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SessionManager.Instance.IsLoggedIn)
            {
                Show();
                return;
            }
            Hide();
        }
    }
}