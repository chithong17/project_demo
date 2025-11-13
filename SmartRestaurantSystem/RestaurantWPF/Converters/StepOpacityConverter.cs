using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RestaurantWPF.Converters
{
    public class StepOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0.3;
            string status = value.ToString();
            string current = parameter?.ToString();
            // Bước đang active thì sáng, còn lại mờ
            return status.Equals(current, StringComparison.OrdinalIgnoreCase) ? 1.0 : 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
