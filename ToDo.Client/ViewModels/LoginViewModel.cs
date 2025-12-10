
namespace ToDo.Client.ViewModels
{
    internal class LoginViewModel : IDialogAware
    {
        private IContainerRegistry container;
        public string Title => "Login";
        public DialogCloseListener RequestClose { get; set; }
        public DelegateCommand LoginCommand { get; private set; }

        public LoginViewModel(IContainerRegistry container)
        {
            this.container = container;
            LoginCommand = new DelegateCommand(Login);
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
