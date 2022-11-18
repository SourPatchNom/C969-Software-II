using System.ComponentModel;
using System.Windows;

namespace OwlScheduler.DesktopEdition.ReportingWindows
{
    public partial class ReportsWindow : Window
    {
        private readonly TypesByMonthWindow _typesByMonthWindow = new TypesByMonthWindow();
        private readonly ConsultantScheduleWindow _consultantScheduleWindow = new ConsultantScheduleWindow();
        private readonly CustomReportWindow _customReportWindow = new CustomReportWindow();
        
        public ReportsWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ButtonTypes_OnClick(object sender, RoutedEventArgs e)
        {
            _typesByMonthWindow.Show();
            _typesByMonthWindow.Activate();
            Hide();
        }

        private void ButtonSchedule_OnClick(object sender, RoutedEventArgs e)
        {
            _consultantScheduleWindow.Show();
            _consultantScheduleWindow.Activate();
            Hide();
        }

        private void ButtonCustom_OnClick(object sender, RoutedEventArgs e)
        {
            _customReportWindow.Show();
            _customReportWindow.Activate();
            Hide();
        }
    }
}