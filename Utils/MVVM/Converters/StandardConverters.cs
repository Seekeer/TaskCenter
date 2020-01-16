using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Utils.MVVM.Converters
{
    /// <summary>
    /// Convert Protocol enum value to BitmapImage protocol's icon.
    /// </summary>
    [ValueConversion(typeof(object), typeof(object))]
    public class NotNullToBoolConverter : Converter<NotNullToBoolConverter>
    {
        #region IValueConverter members

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        #endregion
    }
}
