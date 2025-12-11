

using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ToDo.Client.Login.ViewModels
{
    internal class LoginViewModel : IDialogAware
    {
        private IContainerRegistry container;
        public string Title => "Login";
        public DialogCloseListener RequestClose { get; set; }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand ChangeThemeCommand { get; private set; }

        public LoginViewModel(IContainerRegistry container)
        {
            this.container = container;
            LoginCommand = new DelegateCommand(Login);
            ChangeThemeCommand = new DelegateCommand(ChangeTheme);

        }

        private void ChangeTheme()
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica);
        }

        private void Login()
        {
            //var prms = new DialogParameters();
            //prms.Add()
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
