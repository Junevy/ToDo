using Prism.Mvvm;

namespace ToDo.Client.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly ILocalizationService localizationService;
        public DelegateCommand<bool?> ChangeLangCommand { get; private set; }

        public SettingsViewModel(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            ChangeLangCommand = new(ChangeLang);
        }

        private void ChangeLang(bool? culture)
        {
            localizationService.SetCulture(culture ?? false ? "en-Us" : "zh-CN");
        }

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
