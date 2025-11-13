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
    public class BoolToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isCompleted = System.Convert.ToBoolean(value);
            return isCompleted ? new SolidColorBrush(Color.FromRgb(40, 167, 69)) // xanh lá
                               : new SolidColorBrush(Color.FromRgb(200, 200, 200)); // xám nhạt
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
