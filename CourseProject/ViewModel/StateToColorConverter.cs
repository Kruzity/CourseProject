using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CourseProjectUserApplication.Model;

namespace CourseProjectUserApplication.ViewModel
{
    internal class StateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((RequestState)value)
            {
                case RequestState.Viewed:
                    {
                        return "#4995B6";
                    }
                case RequestState.Unviewed:
                    {
                        return "#B6B6B6";
                    }
                case RequestState.Processing:
                    {
                        return "#E3BF5B";
                    }
                case RequestState.Done:
                    {
                        return "#07B695";
                    }
                default:
                    {
                        return "Black";
                    }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
