using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDo.Extensions.Converter
{
    public class StateToColorConverter : IValueConverter
    {
        public BrushConverter converter = new();


        readonly SolidColorBrush priorityBrush;
        readonly SolidColorBrush completedBrush;
        readonly SolidColorBrush discardBursh;
        readonly SolidColorBrush remindMeBrush;

        public StateToColorConverter()
        {
            priorityBrush = converter.ConvertFromString("#FFE4E1") as SolidColorBrush;
            completedBrush = converter.ConvertFromString("#F0FFF0") as SolidColorBrush;
            discardBursh = converter.ConvertFromString("#F5F5F5") as SolidColorBrush;
            remindMeBrush = converter.ConvertFromString("#F0F8FF") as SolidColorBrush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return discardBursh;
            }
            var status = (int)value;

            return status switch
            {
                0 => completedBrush,
                1 => priorityBrush,
                2 => discardBursh,
                3 => remindMeBrush,
                _ => discardBursh,
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
