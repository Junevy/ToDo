using System.Collections.ObjectModel;
using System.Windows.Input;
using ToDo.Client.Views;
using Wpf.Ui.Controls;

namespace ToDo.Client
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionmanager;
        public ObservableCollection<object> MenuItems { get; set; } = [];
        public ObservableCollection<object> FooterMenuItems { get; set; } = [];

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionmanager = regionManager;

            MenuItems.Add(AddItem(
                "Home",
                new SymbolIcon { Symbol = SymbolRegular.Home24 },
                typeof(HomeView),
                ToHomeCommand));

            FooterMenuItems.Add(AddItem(
                "SET",
                new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                typeof(SettingsView),
                ToSettingsCommand));
        }

        public DelegateCommand ToSettingsCommand => new(() =>
        {
            regionmanager.RequestNavigate("ContentRegion", "SettingsView");
        });
        public DelegateCommand ToHomeCommand => new(() =>
        {
            regionmanager.RequestNavigate("ContentRegion", "HomeView");
        });

        public NavigationViewItem AddItem(string content, SymbolIcon icon, Type targetType, ICommand command)
        {
            return new NavigationViewItem()
            {
                Content = content,
                Icon = icon,
                TargetPageType = targetType,
                Command = command
            };
        }


    }
}
