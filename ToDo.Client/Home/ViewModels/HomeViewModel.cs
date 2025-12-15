using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ObservableCollection<CardControl> CardExpanders { get; set; } = [];

        public HomeViewModel()
        {
            CardExpanders.Add(new CardControl()
            {
                Content = "hello"
            });
        }
    }
}
