using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Login.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly NotificationService notify;
        private readonly AccountService accountService;

        public string Title => "Login";
        public DialogCloseListener RequestClose { get; set; }

        #region Commands
        public AsyncDelegateCommand LoginCommand { get; private set; }
        public AsyncDelegateCommand SignInCommand { get; private set; }
        #endregion

        #region Properties
        private UserInfoRequestDTO accountDTO = new();
        public UserInfoRequestDTO AccountDTO
        {
            get => accountDTO;
            set
            {
                accountDTO = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public LoginViewModel(NotificationService notify, AccountService accountService)
        {
            this.notify = notify;
            this.accountService = accountService;
            LoginCommand = new AsyncDelegateCommand(Login);
            SignInCommand = new(SingIn);
        }

        private async Task SingIn()
        {
            // 基本验证
            if (string.IsNullOrEmpty(AccountDTO.Account)
                || string.IsNullOrEmpty(AccountDTO.Password)
                || AccountDTO.Password != AccountDTO.ConfirmPassword)
            {
                await notify.ShowAsync(TitleType.Error, "Account or Password can not be empty or Password not match!");
                return;
            }

            var result = await accountService.Regeister(AccountDTO);

            // 注册失败
            if (!result)
            {
                await notify.ShowAsync(TitleType.Error, "Sign in error!");
                return;
            }

            // 注册成功
            await notify.ShowAsync(TitleType.Notification, $"Sign in successful! You're Username is {AccountDTO.Account}");
        }

        private async Task Login()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            return;

            // 基本验证
            if (string.IsNullOrEmpty(AccountDTO.Account) || string.IsNullOrEmpty(AccountDTO.Password))
            {
                await notify.ShowAsync(TitleType.Error, "Account or Password can not be empty or Password not matched!");
                return;
            }

            var result = await accountService.Login(accountDTO.Account, AccountDTO.Password);

            if (!result)
            {
                await notify.ShowAsync(TitleType.Error, "Username or password not matched!");
                return;
            }
            // 登录成功
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
