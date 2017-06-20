using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ToolSupervisor.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToolStatus)
            {
                switch ((ToolStatus)value)
                {
                    case ToolStatus.Running:
                        return Brushes.Green;

                    case ToolStatus.Stopped:
                        return Brushes.Red;

                    case ToolStatus.Unknown:
                        return Brushes.Yellow;

                    case ToolStatus.NotFound:
                        return Brushes.Red;
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
