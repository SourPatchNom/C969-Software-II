using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Threading;
using OwlScheduler.DesktopEdition.ManageAppointmentWindows;
using OwlScheduler.DesktopEdition.ManageCustomerWindows;
using OwlScheduler.DesktopEdition.ReportingWindows;
using OwlScheduler.Library.OwlDatabase;
using OwlScheduler.Library.OwlLogger;
using OwlScheduler.Library.OwlSchedule;
using OwlScheduler.Library.OwlSchedule.Classes;

namespace OwlScheduler.DesktopEdition
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ManageCustomerWindow _manageCustomerWindow = new ManageCustomerWindow();
        private readonly ManageAppointmentWindow _manageAppointmentWindow = new ManageAppointmentWindow();
        private readonly WindowUserLogin _windowUserLogin = new WindowUserLogin();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly ReportsWindow _reportsWindow = new ReportsWindow();
        private bool _nextTimerAlert = true;
        
        public MainWindow()
        {

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LogHandler.LogMessage("Main Window", "Initialized.");
            Library.OwlSchedule.OwlScheduler.Instance.Initialize();
            AppointmentsAll.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster;
            TodayDataGrid.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsDay;
            _timer.Tick += OnTickTimeUpdate;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            
            if (!DatabaseHandler.Instance.Initialize(ConfigurationManager.ConnectionStrings["localdb"].ConnectionString, out var connectionResult))
            {
                var result = MessageBox.Show("Connection Error!\n" + connectionResult, "Connection Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }        
            }
            
            CurrentSession.Instance.PropertyChanged += LoginOnPropertyChanged;

            Hide();
            _windowUserLogin.Show();
            _windowUserLogin.Activate();
        }

        private void LoginOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _windowUserLogin.UsernameInput.Clear();
            _windowUserLogin.PasswordInput.Clear();
            _nextTimerAlert = true;
            AppointmentsAll.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster;
            LabelViewMode.Content = "All Your Appointments!";
            if (CurrentSession.Instance.IsLoggedIn)
            {
                Show();
                Activate();
                LabelWelcome.Content = "Hello " + CurrentSession.Instance.CurrentUser?.UserName;
                return;
            }
            Hide();
        }
        
        private void OnTickTimeUpdate(object source, EventArgs e)
        {
            LabelNow.Content = DateTime.Now.ToString("G");

            if (Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentNextAppointment == null)
            {
                LabelNextApptTime.Content = " ";
                return;
            }

            var timeTillNext = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentNextAppointment.StartDateTime - DateTime.Now;

            if (timeTillNext.TotalSeconds < 0)
            {
                Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.UpdateNowAndNext();
                _nextTimerAlert = true;
                return;
            }
            
            if (timeTillNext.TotalMinutes < 15 && _nextTimerAlert)
            {
                if (Application.Current.MainWindow != null) MessageBox.Show(Application.Current.MainWindow, "You have an appointment in the next 15 minutes!", "Reminder!", MessageBoxButton.OK, MessageBoxImage.Information);
                _nextTimerAlert = false;
            }
            LabelNextApptTime.Content = "Next appointment in: " + timeTillNext.ToString(@"h\:mm\:ss");
        }

        private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)
        {
            CurrentSession.Instance.Logout();
            Hide();
            _windowUserLogin.Show();
            _windowUserLogin.Activate();
        }

        private void ButtonViewAll_OnClick(object sender, RoutedEventArgs e)
        {
            AppointmentsAll.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMaster;
            LabelViewMode.Content = "All Your Appointments!";
        }

        private void ButtonViewByMonth_OnClick(object sender, RoutedEventArgs e)
        {
            AppointmentsAll.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsMonth;
            LabelViewMode.Content = "Your Appointments This Month!";
        }

        private void ButtonViewByWeek_OnClick(object sender, RoutedEventArgs e)
        {
            AppointmentsAll.ItemsSource = Library.OwlSchedule.OwlScheduler.Instance.AppointmentDataModel.CurrentUserAppointmentsWeek;
            LabelViewMode.Content = "Your Appointments This Week!";
        }

        private void ButtonSettings_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Thank for reviewing my application!");
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonCustomers_OnClick(object sender, RoutedEventArgs e)
        {
            _manageCustomerWindow.Show();
            _manageCustomerWindow.Activate();
        }

        private void ButtonAppointments_OnClick(object sender, RoutedEventArgs e)
        {
            _manageAppointmentWindow.Show();
            _manageAppointmentWindow.Activate();
        }

        private void ButtonReports_OnClick(object sender, RoutedEventArgs e)
        {
            _reportsWindow.Show();
            _reportsWindow.Activate();
        }
    }
}