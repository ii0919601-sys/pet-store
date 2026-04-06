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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Приложение;
using Приложение.Models;


namespace Приложение
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class GuestPage : Page
    {
        public User CurrentUser { get; set; }
        private List<Product> allProducts;

        public GuestPage()
        {
            InitializeComponent();
            Loadproducts();

        }

        private void Loadproducts()
        {
            Database db = new Database();
            var products = db.GetProducts();
            allProducts = products;
            productsList.ItemsSource = allProducts;

        }
      
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Loadproducts();
            if (CurrentUser != null)
            {
                txtRole.Text = CurrentUser.FullName;
            }
            else
            {
                txtRole.Text = "Гость";
            }
        }
        private void btnCancek_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            NavigationService.Navigate(loginPage);
            NavigationService.RemoveBackEntry();
        }

    }
}
