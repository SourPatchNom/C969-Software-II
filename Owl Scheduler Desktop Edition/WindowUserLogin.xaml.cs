using System.ComponentModel;
using System.Globalization;
using System.Windows;
using OwlSchedulerLibrary;

namespace Owl_Scheduler_Desktop_Edition
{
    public partial class WindowUserLogin : Window
    {
        private int _attempts = 0;
        private string _errorFirst;
        private string _errorSecond;
        
        public WindowUserLogin()
        {
            InitializeComponent();
            CheckLocalization();
        }

        private void CheckLocalization()
        {
            switch (CultureInfo.CurrentCulture.Name)
            {
                //English
                case "en-US":
                case "en":
                case { } a when a.Contains("en"):
                    TitleLabel.Content = "Please Login to Continue!";
                    UsernameLabel.Content = "Username";
                    PasswordLabel.Content = "Password";
                    _errorFirst = "The provided credentials are not recognized! You have used ";
                    _errorSecond = " out of 10 attempts!";
                    break;
                //French
                case "fr-FR":
                case "fr":
                case { } a when a.Contains("fr"):
                    TitleLabel.Content = "Veuillez Vous Connecter";
                    UsernameLabel.Content = "Nom d'utilisateur";
                    PasswordLabel.Content = "Mot de passe";
                    _errorFirst = "Les informations d'identification fournies ne sont pas reconnues ! Vous avez utilisé ";
                    _errorSecond = " sur 10 tentatives !";
                    break;
                //Spanish
                case "es-ES":
                case "es":
                case { } a when a.Contains("es"):
                    TitleLabel.Content = "Por favor inicie sesión para continuar";
                    UsernameLabel.Content = "Nombre de usuario";
                    PasswordLabel.Content = "Clave";
                    _errorFirst = "¡Las credenciales proporcionadas no son reconocidas! has usado ";
                    _errorSecond = " de 10 intentos!!";
                    break;
            }
        }

        private void WindowUserLogin_OnClosing(object sender, CancelEventArgs e)
        {
            if (!SessionManager.Instance.IsLoggedIn || _attempts >= 10)
            {
                Application.Current.Shutdown();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (SessionManager.Instance.ProcessLoginAttempt(UsernameInput.Text,PasswordInput.Text))
            {
                Hide();
                return;
            }

            _attempts++;
            
            if (_attempts >= 10)
            {
                Close();
                Application.Current.Shutdown();
                return;
            }
            
            MessageBox.Show(_errorFirst + _attempts + _errorSecond, "Unable to Login!",MessageBoxButton.OK,MessageBoxImage.Error);
        }
    }
}