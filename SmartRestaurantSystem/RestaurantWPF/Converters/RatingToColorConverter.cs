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
    public class RatingToColorConverter : IMultiValueConverter
    {
        // values[0] = SelectedRating
        // values[1] = Star index (1..5)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is int rating && values[1] is int currentStar)
            {
                return rating >= currentStar
                    ? Brushes.Gold
                    : Brushes.LightGray;
            }
            return Brushes.LightGray;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
