using System.Globalization;
using System.Windows;
using ToDo.Client.Login.ViewModels;
using ToDo.Client.Login.Views;
using ToDo.Client.Services;
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
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<LoginView>();
            containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();
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
