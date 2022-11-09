using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using OwlSchedulerLibrary.OwlSchedule.DataModel;

namespace Owl_Scheduler_Desktop_Edition.ReportingWindows
{
    public partial class CustomReportWindow : Window
    {
        public CustomReportWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            ComboMonth.ItemsSource = Enumerable.Range(1,12);
            ComboMonth.SelectedItem = DateTime.Now.Month;
            ComboYear.ItemsSource = Enumerable.Range(DateTime.Now.Year-10,20);
            ComboYear.SelectedItem = DateTime.Now.Year;
        }

        private void Calculate()
        {
            LabelCount.Content = "0";
            if (TextBoxLocation.Text == " ")
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Input a type to search for!";
                return;
            }
            
            if (ComboMonth.SelectedIndex == -1)
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Pick a month!";
                return;
            }
            
            if (ComboYear.SelectedIndex == -1)
            {
                LabelError.Visibility = Visibility.Visible;
                LabelError.Content = "Pick a year!";
                return;
            }
            
            LabelError.Visibility = Visibility.Hidden;
            LabelError.Content = "";
            LabelCount.Content = AppointmentDataModel.GetLocationCountByNameGivenMonthYear(TextBoxLocation.Text,(int)ComboMonth.SelectedItem,(int)ComboYear.SelectedItem);
        }
        
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Calculate();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Calculate();
        }
    }
}