using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using Приложение.Models;

namespace Приложение
{
    public class Database
    {
        public void AddUser(string login, string password, string fullName)
        {
            string connectionString = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO user_import ([Роль сотрудника], ФИО, Логин, Пароль) VALUES (@role, @fullName, @login, @password)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@fullName", fullName);
                cmd.Parameters.AddWithValue("@role", "@user");
                cmd.ExecuteNonQuery();
            }
        }
        public bool IsLoginExists(string login)
        {
            string connection = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM user_import WHERE Логин = @login";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@login", login);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        public User GetUser(string login, string password)
        {
            string connection = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "SELECT * FROM user_import WHERE Логин = @login AND Пароль = @password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);

                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        
                        Login = reader["Логин"].ToString(),
                        FullName = reader["ФИО"].ToString(),
                        Role = reader["Роль сотрудника"].ToString()
                    };
                }
            }
            return null;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            string connectionString = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tovar";

                var command = new SqlCommand(query, conn);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product p = new Product();
                    p.Id = (int)reader["ID"];
                    p.Article = reader["Артикул"].ToString();
                    p.Name = reader["Наименование товара"].ToString();
                    p.Unit = reader["Единица измерения"].ToString();
                    p.Price = (decimal)reader["Цена"];
                    p.Supplier = reader["Поставщик"].ToString();
                    p.Manufacturer = reader["Производитель"].ToString();
                    p.Category = reader["Категория товара"].ToString();
                    p.Quantity = (int)reader["Кол-во на складе"];
                    p.Discount = (int)reader["Действующая скидка"];
                    p.Description = reader["Описание товара"].ToString();
                    p.ImagePath = reader["Фото"].ToString();
                    products.Add(p);


                }
            }
            return products;
        }
        public void AddProduct(Product product)
        {
            string connection = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "INSERT INTO Tovar(Артикул, [Наименование товара], Цена, Поставщик," +
                    "Производитель, [Категория товара], [Кол-во на складе], [Действующая скидка], [Описание товара]," +
                    "[Единица измерения], Фото) VALUES (@Article, @Name, @Price, @Supplier, @Manufacturer, @Category, @Quantity, @Discount," +
                    "@Description, @Unit ,@ImagePath)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Article", product.Article);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Supplier", product.Supplier);
                cmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
                cmd.Parameters.AddWithValue("@Category", product.Category);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Discount", product.Discount);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                cmd.Parameters.AddWithValue("@Unit", product.Unit);
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteProduct(string article)
        {
            string connection = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM Tovar WHERE Артикул = @article";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@article", article);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            string connection = "Data Source=PLABSQLW19S1,49172;Initial Catalog=Зверополис;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                string query = "UPDATE Tovar SET " +
                    "Артикул = @Article, " +
                    "[Наименование товара] = @Name, " +
                    "Цена = @Price, " +
                    "[Кол-во на складе] = @Quantity, " +
                    "Поставщик = @Supplier, " +
                    "Производитель = @Manufacturer, " +
                    "[Категория товара] = @Category, " +
                    "[Описание товара] = @Description, " +
                    "[Единица измерения] = @Unit, " +
                    "[Действующая скидка] = @Discount, " +
                    "Фото = @ImagePath " +
                    "WHERE Артикул = @OldArticle";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Article", product.Article);
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Supplier", product.Supplier);
                cmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
                cmd.Parameters.AddWithValue("@Category", product.Category);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Discount", product.Discount);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                cmd.Parameters.AddWithValue("@Unit", product.Unit);
                cmd.Parameters.AddWithValue("@OldArticle", product.Article);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
