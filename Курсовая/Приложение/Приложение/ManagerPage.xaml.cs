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
using System.ComponentModel;

namespace Приложение
{
    public partial class ManagerPage : Page
    {
        private List<Product> allProducts;
        private string currentFilter = "Все поставщики";
        private string currentSort = "";
        private string currentSearchText = "";
        public User CurrentUser { get; set; }

        private CollectionViewSource productsViewSource;
        private ICollectionView productsView;

        public ManagerPage()
        {
            InitializeComponent();
        }

        private void Loadproducts()
        {
            Database db = new Database();
            allProducts = db.GetProducts();

            List<string> suppliers = new List<string> { "Все поставщики" };
            foreach (Product p in allProducts)
            {
                if (!suppliers.Contains(p.Supplier))
                {
                    suppliers.Add(p.Supplier);
                }
            }
            cmbManufacturer.ItemsSource = suppliers;

            productsViewSource = new CollectionViewSource { Source = allProducts };
            productsView = productsViewSource.View;
            productsView.Filter = ProductsFilter;
            productsList.ItemsSource = productsView;
        }

        private bool ProductsFilter(object item)
        {
            if (item is Product product)
            {
                bool searchMatch = string.IsNullOrEmpty(currentSearchText) ||
                    product.Name.ToLower().Contains(currentSearchText) ||
                    product.Category.ToLower().Contains(currentSearchText);

                bool filterMatch = currentFilter == "Все поставщики" || product.Supplier == currentFilter;

                return searchMatch && filterMatch;
            }
            return false;
        }

        private List<string> GetUniqueManufacturers()
        {
            return allProducts?.Select(p => p.Manufacturer).Distinct().ToList() ?? new List<string>();
        }

        private List<string> GetUniqueCategories()
        {
            return allProducts?.Select(p => p.Category).Distinct().ToList() ?? new List<string>();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (productsView == null) return;
            currentSearchText = txtSearch.Text.ToLower();
            productsView.Refresh();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Найти товар...")
            {
                txtSearch.Text = "";
                currentSearchText = "";
                if (productsView != null) productsView.Refresh();
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Найти товар...";
                currentSearchText = "";
                if (productsView != null) productsView.Refresh();
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSort.SelectedItem == null || productsView == null) return;
            currentSort = (cmbSort.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";

            if (productsView.CanSort)
            {
                productsView.SortDescriptions.Clear();
                if (currentSort == "По кол-ву (возр.)")
                    productsView.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Ascending));
                else if (currentSort == "По кол-ву (убыв.)")
                    productsView.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Descending));
            }
            productsView.Refresh();
        }

        private void cmbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (productsView == null) return;
            currentFilter = cmbManufacturer.SelectedItem?.ToString() ?? "Все поставщики";
            productsView.Refresh();
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            NavigationService.Navigate(loginPage);
            NavigationService.RemoveBackEntry();
        }
    }
}
