using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace RestaurantWPF.Converters
{
    public class OrderStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Gray;

            int status = System.Convert.ToInt32(value);
            return status switch
            {
                0 => new SolidColorBrush(Color.FromRgb(255, 183, 77)),   // cam nhạt
                1 => new SolidColorBrush(Color.FromRgb(255, 152, 0)),    // cam đậm
                2 => new SolidColorBrush(Color.FromRgb(33, 150, 243)),   // xanh dương
                3 => new SolidColorBrush(Color.FromRgb(76, 175, 80)),    // xanh lá
                4 => new SolidColorBrush(Color.FromRgb(244, 67, 54)),    // đỏ
                _ => Brushes.Gray
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
