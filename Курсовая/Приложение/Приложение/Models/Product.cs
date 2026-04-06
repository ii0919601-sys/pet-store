using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Приложение.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public string Supplier { get; set; }
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public decimal PriceWithDiscount
        {
            get { return Price * (100 - Discount) / 100; }
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
