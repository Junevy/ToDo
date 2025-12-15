using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ToDo.Client.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly ILocalizationService localizationService;

        private bool theme = false;
        public bool Theme
        {
            get { return theme; }
            set { SetProperty(ref theme, value); }
        }

        private bool lang = true;
        public bool Lang
        {
            get { return lang; }
            set { SetProperty(ref lang, value); }
        }


        public DelegateCommand<string> ChangeLangCommand { get; private set; }
        public DelegateCommand<string> ChangeThemeCommand => new((theme) =>
        {
            ApplicationThemeManager.Apply(
                theme == "Light" ? ApplicationTheme.Light :
                ApplicationTheme.Dark, WindowBackdropType.Mica);
        });

        //public string test { get; set; } = "Settings ViewModel";

        public SettingsViewModel(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            ChangeLangCommand = new(ChangeLang);
        }

        private void ChangeLang(string culture)
            => localizationService.SetCulture(culture == "English" ? "en-Us" : "zh-CN");

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
