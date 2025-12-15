using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.HttpClient;
using ToDo.WebAPI.Request;

namespace ToDo.Client.Login.ViewModels
{
    internal class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly IContainerRegistry container;
        private readonly HttpRequestClient httpClient;

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

        public LoginViewModel(IContainerRegistry container, HttpRequestClient client)
        {
            this.container = container;
            this.httpClient = client;
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
                var message = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = "Account or Password can not be empty or Password not match!"
                };
                _ = message.ShowDialogAsync();
                return;
            }

            // 创建请求，设定 路由、请求方式、DTO
            var request = new AccountRequest<AccountDTO>
            {
                Route = "/Users/Register",
                Method = RestSharp.Method.POST,
                Params = accountDTO
            };

            // 发起请求
            var response = await httpClient.ExecuteAsync(request);
            // 注册失败
            if (response.Code != 1)
            {
                var signInError = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = response.Message ?? "Sign in error!"
                };
                _ = signInError.ShowDialogAsync();
                return;
            }

            // 注册成功
            var signInScs = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Notification",
                Content = response.Message
            };
            _ = signInScs.ShowDialogAsync();
        }

        private async Task Login()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            return;

            // 基本验证
            if (string.IsNullOrEmpty(AccountDTO.Account) || string.IsNullOrEmpty(AccountDTO.Password))
            {
                var message = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = "Account or Password can not be empty or Password not matched!"
                };
                _ = message.ShowDialogAsync();
                return;
            }

            // 创建请求，设定 路由、请求方式、DTO
            var request = new AccountRequest<AccountDTO>
            {
                Route = $"/Users/Login?account={accountDTO.Account}&password={accountDTO.Password}",
                Method = RestSharp.Method.GET,
            };

            // 发起请求
            var response = await httpClient.ExecuteAsync(request);
            // 登录失败
            if (response.Code != 1)
            {
                var signInError = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = response.Message ?? "Login in error!"
                };
                _ = signInError.ShowDialogAsync();
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
