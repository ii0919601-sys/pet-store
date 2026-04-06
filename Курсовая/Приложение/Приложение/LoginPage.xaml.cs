using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Приложение;
namespace Приложение
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }    
        private void SignGuestButton_Click(object sender, RoutedEventArgs e)
        {
            GuestPage guestPage = new GuestPage();
            guestPage.CurrentUser = new Models.User { FullName = "Гость", Role = "guest" };
            NavigationService.Navigate(guestPage);
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PassLogin.Password;
           if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            Database db = new Database();
            var user = db.GetUser(login, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            switch (user.Role)
            {                               
                case "Авторизованный пользователь":
                    UserPage userPage = new UserPage();
                    userPage.CurrentUser = user;
                    NavigationService.Navigate(userPage);
                    break;

                case "Менеджер":
                    ManagerPage managerPage = new ManagerPage();
                    managerPage.CurrentUser = user;
                    NavigationService.Navigate(managerPage);
                    break;

                case "Администратор":
                    AdminPage adminPage = new AdminPage();
                    adminPage.CurrentUser = user;
                    NavigationService.Navigate(adminPage);
                    break;

                default:
                    GuestPage guestPage = new GuestPage();
                    guestPage.CurrentUser = user;
                    NavigationService.Navigate(guestPage);
                    break;
                    
            }
        }


        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Логин")
            {
                LoginTextBox.Text = "";
            }  
        }
        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.Text = "Логин";
            }
        }

        private void PassLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            PassPlaceholder.Visibility = Visibility.Collapsed;
        }
        private void PassLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PassLogin.Password))
            {
                PassPlaceholder.Visibility = Visibility.Visible;
            }
        }
        
    }
}
