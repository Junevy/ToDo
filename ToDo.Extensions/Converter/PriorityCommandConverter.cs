using System.Globalization;
using System.Windows.Data;

namespace ToDo.Extensions.Converter
{
    public class PriorityCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string test = string.Empty;
            foreach (var item in values)
            {
                test = item + ";";
            }
            return test;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
