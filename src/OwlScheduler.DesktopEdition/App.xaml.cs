using System.Configuration;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Animation;
using OwlScheduler.Library;
using OwlScheduler.Library.OwlLogger;

namespace OwlScheduler.DesktopEdition
{
    
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        App()
        {
            bool needRestart = false;
            needRestart = LogHandler.Initialize();
            if (needRestart)
            {
                var reply = MessageBox.Show("Log file created, please restart application!","Hey!",MessageBoxButton.OK,MessageBoxImage.Error);
                if (reply != MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }    
        }
    }
}