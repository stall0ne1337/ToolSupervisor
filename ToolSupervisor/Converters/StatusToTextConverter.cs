using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ToolSupervisor.Converters
{
    public class StatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToolStatus)
            {
                switch ((ToolStatus)value)
                {
                    case ToolStatus.Running:
                        return "Running";

                    case ToolStatus.Stopped:
                        return "Stopped";

                    case ToolStatus.Unknown:
                        return "Checking..";

                    case ToolStatus.NotFound:
                        return "Not Found";
                }
            }

            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
