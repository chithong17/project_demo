using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestaurantWPF.Converters
{
    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "Không rõ";

            int status = System.Convert.ToInt32(value);
            return status switch
            {
                0 => "Đặt trước",
                1 => "Chờ xác nhận",
                2 => "Đang phục vụ",
                3 => "Hoàn tất",
                4 => "Đã hủy",
                _ => "Không rõ"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
