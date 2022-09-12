using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using OwlSchedulerLibrary;
using OwlSchedulerLibrary.Database;

namespace Owl_Scheduler_Desktop_Edition
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly WindowUserLogin _windowUserLogin = new WindowUserLogin();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        
        
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LogHandler.Instance.LogMessage("Main Window", "Initialized.");
            OwlScheduler.Instance.Initialize();
            
            _timer.Tick += OnTickTimeUpdate;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            
            if (!DatabaseHandler.Instance.Initialize(ConfigurationManager.ConnectionStrings["devdb"].ConnectionString, out var connectionResult))
            {
                var result = MessageBox.Show("Connection Error!\n" + connectionResult, "Connection Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }        
            }
            SessionManager.Instance.PropertyChanged += LoginOnPropertyChanged;
            //TODO enable for release!
            // Hide();
            // _windowUserLogin.Show();
            // _windowUserLogin.Activate();

            
            SessionManager.Instance.ProcessLoginAttempt("test", "test");
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _windowUserLogin.UsernameInput.Clear();
            _windowUserLogin.PasswordInput.Clear();
            if (SessionManager.Instance.IsLoggedIn)
            {
                
                Show();
                LabelWelcome.Content = "Hello " + SessionManager.Instance.CurrentUser;
                return;
            }
            Hide();
        }
        
        private void OnTickTimeUpdate(object source, EventArgs e)
        {
            LabelNow.Content = DateTime.Now.ToString("G");
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            SessionManager.Instance.Logout();
            Hide();
            _windowUserLogin.Show();
            _windowUserLogin.Activate();
        }
    }
}