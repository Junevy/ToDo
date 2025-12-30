using System.Globalization;
using System.Windows;
using ToDo.Client.Home.ViewModels;
using ToDo.Client.Home.Views;
using ToDo.Client.Login.Views;
using ToDo.Client.Overview.ViewModels;
using ToDo.Client.Overview.Views;
using ToDo.Client.Services;
using ToDo.Client.Settings.ViewModels;
using ToDo.Client.Settings.Views;
using ToDo.WebAPI.Client;
using ToDo.WebAPI.Services;
using ToDo.WebAPI.Services.Interface;
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
            // Views and ViewModels
            // ===================================
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<LoginView>();
            containerRegistry.RegisterDialog<AddPriorityView, AddPriorityViewModel>();

            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<OverviewView, OverviewViewModel>();

            // ===================================
            // Service 
            // ===================================
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
            containerRegistry.RegisterSingleton<ISnackbarService, SnackbarService>();
            containerRegistry.RegisterSingleton<IApi, HttpService>();
            containerRegistry.RegisterSingleton<PriorityService>();
            containerRegistry.RegisterSingleton<AccountService>();
            containerRegistry.RegisterSingleton<RequestClient>(_ => new RequestClient("http://localhost:6338/api"));

            // ===================================
            // Tools
            // ===================================
            //containerRegistry.GetContainer().Register<HttpRequestClient<AccountDTO>>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            //containerRegistry.GetContainer().Register<HttpRequestClient<PriorityDTO>>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));
            //containerRegistry.GetContainer().Register<HttpRequestClient<MainInfoDTO>>(made: Parameters.Of.Type<string>(serviceKey: "webUrl"));

        }

        protected override void OnInitialized()
        {
            var culture = new CultureInfo("en-US");
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

            //_ = CheckServerAsync();


            base.OnInitialized();
        }

        protected override void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            base.ConfigureViewModelLocator();
        }

        public async Task<bool> CheckServerAsync()
        {
            try
            {
                var tempApi = Container.Resolve<IApi>();

                var response = await tempApi.GetRequestAsync<string>("/health");
                return response.IsSuccess;
            }
            catch
            {
                return false;
            }
        }
    }
}
