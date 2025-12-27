using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToDo.Client.Settings.Views;
using ToDo.Client.Home.Views;
using Wpf.Ui.Controls;

namespace ToDo.Client
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionmanager;
        public ObservableCollection<object> MenuItems { get; set; } = [];
        public ObservableCollection<object> FooterMenuItems { get; set; } = [];
        public DelegateCommand ToSettingsCommand { get; set; }

        public DelegateCommand ToHomeCommand { get; set; }
        public DelegateCommand ToOverviewCommand { get; set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionmanager = regionManager;

            ToSettingsCommand = new(ToSettings);
            ToHomeCommand = new(ToHome);
            ToOverviewCommand = new(ToOverview);

            MenuItems.Add(AddItem(
                "Home",
                new SymbolIcon { Symbol = SymbolRegular.Home24 },
                typeof(HomeView),
                ToHomeCommand));

            MenuItems.Add(AddItem(
                "Over",
                new SymbolIcon { Symbol = SymbolRegular.ContentViewGallery24 },
                typeof(HomeView),
                ToOverviewCommand));

            FooterMenuItems.Add(AddItem(
                "SET",
                new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                typeof(SettingsView),
                ToSettingsCommand));

            FooterMenuItems.Add(AddItem(
                "Admin",
                new ImageIcon
                {
                    Source = new BitmapImage(
                    new Uri("pack://application:,,,/ToDo.Resources;component/AppIcon/users.jpg")),
                    Width = 40,
                    Height = 40,
                    Clip = new EllipseGeometry()
                    {
                        Center = new System.Windows.Point(20, 20),
                        RadiusX = 20,
                        RadiusY = 20,
                    }
                },
                typeof(SettingsView),
                ToHomeCommand));
        }

        private void ToOverview()
            => regionmanager.RequestNavigate("ContentRegion", "OverviewView");
        private void ToSettings()
            => regionmanager.RequestNavigate("ContentRegion", "SettingsView");
        private void ToHome()
            => regionmanager.RequestNavigate("ContentRegion", "HomeView");


        /// <summary>
        /// Add Navigation item with symbol icon
        /// </summary>
        /// <param name="content"></param>
        /// <param name="icon"></param>
        /// <param name="targetType"></param>
        /// <param name="command"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add Navigation item with image icon
        /// </summary>
        /// <param name="content"></param>
        /// <param name="icon"></param>
        /// <param name="targetType"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public NavigationViewItem AddItem
            (string content, ImageIcon icon, Type targetType, ICommand command)
        {
            return new NavigationViewItem()
            {
                Content = content,
                TargetPageType = targetType,
                Icon = icon,
                Command = command,
            };
        }
    }
}
