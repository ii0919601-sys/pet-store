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
using Приложение.Models;

namespace Приложение
{
    /// <summary>
    /// Логика взаимодействия для EditProductPage.xaml
    /// </summary>
    public partial class EditProductPage : Page
    {      
        
        public List<string> Categories { get; set; }
        public List<string> Manufacturers { get; set; }
        public Product EditingProduct { get; set; }

        public void LoadData()
        {
            ManufacturerComboBox.ItemsSource = Manufacturers;
            CategoryComboBox.ItemsSource = Categories;
            ManufacturerComboBox.SelectedItem = EditingProduct.Manufacturer;
            CategoryComboBox.SelectedItem = EditingProduct.Category;
        }
        
        public EditProductPage(Product product)
        {
            InitializeComponent();        
            EditingProduct = product;
            IdTextBox.Text = product.Id.ToString();
            ArticleTextBox.Text = product.Article;
            NameTextBox.Text = product.Name;
            PriceTextBox.Text = product.Price.ToString();
            QuantityTextBox.Text = product.Quantity.ToString();
            SupplierTextBox.Text = product.Supplier;           
            DescriptionTextBox.Text = product.Description;
            cmdUnit.Text = product.Unit;
            DiscountTextBox.Text = product.Discount.ToString();
            PhotoPathTextBox.Text = product.ImagePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(ArticleTextBox.Text))
                {
                    MessageBox.Show("Введите Артикул товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                {
                    MessageBox.Show("Введите Наименование товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
                {
                    MessageBox.Show("Введите Цену товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Ошибка заполнения поля 'цена товара'", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(QuantityTextBox.Text))
                {
                    MessageBox.Show("Введите количество товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Ошибка заполнения поля 'Количество товара'", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SupplierTextBox.Text))
                {
                    MessageBox.Show("Введите Поставщика товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(ManufacturerComboBox.Text))
                {
                    MessageBox.Show("Введите Производителя товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(CategoryComboBox.Text))
                {
                    MessageBox.Show("Введите Категорию товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
                {
                    MessageBox.Show("Введите Описание товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(cmdUnit.Text))
                {
                    MessageBox.Show("Введите Единицу измерения", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                if (string.IsNullOrWhiteSpace(DiscountTextBox.Text))
                {
                    MessageBox.Show("Введите скидку товара товара", "Ошибка", MessageBoxButton.OK);
                    return;
                }
                if (!int.TryParse(DiscountTextBox.Text, out int discout) || discout < 0 || discout > 100)
                {
                    MessageBox.Show("Ошибка заполнения поля 'Скидка товара'", "Ошибка", MessageBoxButton.OK);
                    return;
                }

                string Article = ArticleTextBox.Text;
                string Name = NameTextBox.Text;
                decimal Price = decimal.Parse(PriceTextBox.Text);
                int Quantity = int.Parse(QuantityTextBox.Text);
                string Supplier = SupplierTextBox.Text;
                string Manufacturer = ManufacturerComboBox.Text;
                string Category = CategoryComboBox.Text;
                string Description = DescriptionTextBox.Text;
                string Unit = cmdUnit.Text;
                string Discount = DiscountTextBox.Text;
                string ImagePath = PhotoPathTextBox.Text;
                if (string.IsNullOrWhiteSpace(PhotoPathTextBox.Text))
                {
                    ImagePath = "Images/plug.jpg";
                }
                else
                {
                    ImagePath = PhotoPathTextBox.Text;
                }
                
                EditingProduct.Article = Article;
                EditingProduct.Name = Name;
                EditingProduct.Price = Price;
                EditingProduct.Quantity = Quantity;
                EditingProduct.Supplier = Supplier;
                EditingProduct.Manufacturer = Manufacturer;
                EditingProduct.Category = Category;
                EditingProduct.Description = Description;
                EditingProduct.Unit = Unit;
                EditingProduct.ImagePath = ImagePath;

                int discountValue = int.Parse(Discount);
                EditingProduct.Discount = discountValue;



                Database db = new Database();
                db.UpdateProduct(EditingProduct);
                NavigationService.GoBack();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                PhotoPathTextBox.Text = openFileDialog.FileName;
            }
        }
    }   
}
