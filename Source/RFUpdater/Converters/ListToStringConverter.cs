using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace RFUpdater.Converters
{
    [ValueConversion(typeof(List<string>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {
        private const string constSeparator = ",";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (targetType != typeof(string))
                    throw new InvalidOperationException("The target must be a String");

                return String.Join(constSeparator, ((List<string>)value).ToArray());
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new List<string>(((string)value).Split(constSeparator.ToCharArray()));
        }
    }
}