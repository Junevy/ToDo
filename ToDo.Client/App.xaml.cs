using System.Globalization;
using System.Windows;
using ToDo.Client.Home.ViewModels;
using ToDo.Client.Home.Views;
using ToDo.Client.Login.Views;
using ToDo.Client.Services;
using ToDo.Client.Settings.ViewModels;
using ToDo.Client.Settings.Views;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.HttpClient;
using Wpf.Ui;
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
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterDialog<AddPriorityView, AddPriorityViewModel>();

            //containerRegistry.RegisterSingleton<SettingsViewModel>();
            //containerRegistry.RegisterSingleton<HomeViewModel>();

            // ===================================
            // Localization service 
            // ===================================
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

            // ===================================
            // Tools
            // ===================================
            containerRegistry.GetContainer().Register<HttpRequestClient<AccountDTO>>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            containerRegistry.GetContainer().Register<HttpRequestClient<PriorityDTO>>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));

            containerRegistry.RegisterSingleton<ISnackbarService, SnackbarService>();
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
