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
    public class TableStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = System.Convert.ToInt32(value);
            return s switch
            {
                0 => "Trống",
                1 => "Đang sử dụng",
                2 => "Đã đặt trước",
                3 => "Đang dọn dẹp",
                4 => "Không hoạt động",
                _ => "Không xác định"
            };
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotImplementedException();
    }

    public class TableStatusBrushConverter : IValueConverter
    {
        private static readonly Brush Green = new SolidColorBrush(Color.FromRgb(67, 160, 71));   // Available
        private static readonly Brush Red = new SolidColorBrush(Color.FromRgb(229, 57, 53));   // Occupied
        private static readonly Brush Amber = new SolidColorBrush(Color.FromRgb(251, 192, 45));  // Reserved
        private static readonly Brush Blue = new SolidColorBrush(Color.FromRgb(30, 136, 229));  // Cleaning
        private static readonly Brush Gray = new SolidColorBrush(Color.FromRgb(117, 117, 117)); // Inactive

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = System.Convert.ToInt32(value);
            return s switch
            {
                0 => Green,
                1 => Red,
                2 => Amber,
                3 => Blue,
                4 => Gray,
                _ => Gray
            };
        }

        public object ConvertBack(object v, Type t, object p, CultureInfo c)
            => throw new NotImplementedException();
    }
}
