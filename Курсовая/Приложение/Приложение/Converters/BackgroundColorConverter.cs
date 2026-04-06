using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Приложение.Models;

namespace Приложение.Converters
{
    public class BackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Product product)
            {
                if (product.Discount > 15)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E8B57"));
                if (product.Quantity == 0)
                    return new SolidColorBrush(Colors.LightBlue);
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}