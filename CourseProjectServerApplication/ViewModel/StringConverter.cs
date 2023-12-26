using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CourseProjectServerApplication.Model;

namespace CourseProjectServerApplication.ViewModel
{
    public class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            RequestState tmp;
            Enum.TryParse<RequestState>(value.ToString(), out tmp);
            return tmp;
        }
    }
}
