using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.HttpClient;
using ToDo.WebAPI.Request;

namespace ToDo.Client.Login.ViewModels
{
    internal class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IContainerRegistry container;
        private readonly HttpService httpService;
        private readonly NotificationService notify;

        public string Title => "Login";
        public DialogCloseListener RequestClose { get; set; }

        #region Commands
        public AsyncDelegateCommand LoginCommand { get; private set; }
        public AsyncDelegateCommand SignInCommand { get; private set; }
        #endregion

        #region Properties
        private AccountDTO accountDTO = new();
        public AccountDTO AccountDTO
        {
            get => accountDTO;
            set
            {
                accountDTO = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public LoginViewModel(IContainerRegistry container, HttpService httpService, NotificationService notify)
        {
            this.container = container;
            this.httpService = httpService;
            this.notify = notify;
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
                await notify.ShowMessageAsync(TitleType.Error, "Account or Password can not be empty or Password not match!");
                return;
            }

            // 创建请求，设定 路由、请求方式、DTO
            var response = await httpService.PostRequestAsync("/Users/Register", accountDTO);

            // 注册失败
            if (response.Code != 1)
            {
                await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Sign in error!");
                return;
            }

            // 注册成功
            await notify.ShowMessageAsync(TitleType.Notification, response.Message);
        }

        private async Task Login()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            return;

            // 基本验证
            if (string.IsNullOrEmpty(AccountDTO.Account) || string.IsNullOrEmpty(AccountDTO.Password))
            {
                await notify.ShowMessageAsync(TitleType.Error, "Account or Password can not be empty or Password not matched!");
                return;
            }

            // 创建请求，设定 路由、请求方式、DTO
            var response = await httpService.GetRequestAsync<AccountDTO>($"/Users/Login?account={accountDTO.Account}&password={accountDTO.Password}");

            // 登录失败
            if (response.Code != 1)
            {
                await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Login in error!");
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
