namespace ToDo.Client
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ILocalizationService localizationService;

        public DelegateCommand<bool?> ChangeLangCommand { get; private set; }

        public MainWindowViewModel(ILocalizationService localizationService)
        {
            this.localizationService = localizationService;
            ChangeLangCommand = new(ChangeLang);
        }

        private void ChangeLang(bool? culture)
        {
            localizationService.SetCulture(culture ?? false ? "en-Us" : "zh-CN");
        }
    }
}
