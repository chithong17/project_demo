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
    public class TableStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte status)
            {
                return status switch
                {
                    0 => new SolidColorBrush(Color.FromRgb(76, 175, 80)),   // 🟢 Trống
                    1 => new SolidColorBrush(Color.FromRgb(255, 193, 7)),   // 🟡 Đang phục vụ
                    2 => new SolidColorBrush(Color.FromRgb(244, 67, 54)),   // 🔴 Bận
                    _ => new SolidColorBrush(Color.FromRgb(189, 189, 189))  // ⚪ Mặc định
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
