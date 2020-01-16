using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.MVVM;

namespace Utils.MVVM.Converters
{
    /// <summary>
    /// Convert contact to contact view model
    /// </summary>
    public class DateToStringConverter : Converter<DateToStringConverter>
    {
        #region IValueConverter members

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is DateTime date)
                return $"{date.ToShortDateString()} {date.ToShortTimeString()}";
            
            Debug.Assert(false);
            return string.Empty;
        }

        #endregion
    }
}
