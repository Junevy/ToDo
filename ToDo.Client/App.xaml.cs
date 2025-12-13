using System.Globalization;
using System.Windows;
using ToDo.Client.Login.Views;
using ToDo.Client.Services;
using ToDo.Client.Views;
using ToDo.WebAPI.HttpClient;
using DialogWindow = ToDo.Client.Login.Views.DialogWindow;

namespace ToDo.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // ===================================
            // Views 
            // ===================================
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<LoginView>();
            containerRegistry.RegisterForNavigation<SettingsView>();
            containerRegistry.RegisterForNavigation<HomeView>();

            // ===================================
            // Localization service 
            // ===================================
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

            // ===================================
            // Tools
            // ===================================
            containerRegistry.GetContainer().Register<HttpRequestClient>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
        }

        protected override void OnInitialized()
        {
            var culture = new CultureInfo("zh-CN");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", (callbackResult) =>
            {
                if (callbackResult.Result != ButtonResult.OK)
                {
                    Environment.Exit(0);
                    return;
                }
            });
            base.OnInitialized();
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            base.ConfigureViewModelLocator();
        }
    }
}
