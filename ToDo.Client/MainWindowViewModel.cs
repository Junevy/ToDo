using System.Collections.ObjectModel;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace ToDo.Client
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        public ObservableCollection<object> MenuItems { get; set; } = [];
        public ObservableCollection<object> FooterMenuItems { get; set; } = [];

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            MenuItems.Add(AddItem(
                "Home",
                new SymbolIcon { Symbol = SymbolRegular.Home24 },
                typeof(Views.SettingsView),
                ToSettingsCommand));

            FooterMenuItems.Add(AddItem(
                "SET",
                new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                typeof(Views.SettingsView),
                ToSettingsCommand));
        }

        public DelegateCommand ToSettingsCommand => new(() =>
        {
            //regionManager.RequestNavigate("ContentRegion", "SettingsView");
        });

        public NavigationViewItem AddItem
            (string content, SymbolIcon icon, Type targetType, ICommand command)
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
