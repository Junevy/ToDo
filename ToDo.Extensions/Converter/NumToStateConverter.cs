using System.Globalization;
using System.Windows.Data;

namespace ToDo.Extensions.Converter
{
    public class NumToStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (int)value;
            //propertychange

            return state switch
            {
                4 => Lang.Lang.ResourceManager.GetString("Normal", culture),
                1 => Lang.Lang.ResourceManager.GetString("PriorityThings", culture),
                _ => Lang.Lang.ResourceManager.GetString("SelectLevel", culture),
            }; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;   
    }
}
