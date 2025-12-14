using System.ComponentModel.DataAnnotations;
using ToDo.WebAPI.Services;

namespace ToDo.WebAPI.DTOs
{
    public class AccountDTO : ValidatableBindableBase
    {
        private string account;
        [Required(ErrorMessage = "Account can not be empty!")]
        //[RegularExpression("")]
        public string Account
        {
            get => account;
            set
            {
                SetProperty(ref account, value);
            }
        }

        private string password;
        [Required(ErrorMessage = "Password can not be empty!")]
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
                if (value != ConfirmPassword)
                {
                    if (!HasErrored(nameof(Password)))
                        AddError(nameof(Password), "Password can not matched!");
                }
                else
                {
                    ClearErrors(nameof(Password));
                    ClearErrors(nameof(Password));
                }
            }
        }

        private string confirmPassword = string.Empty;
        [Required(ErrorMessage = "ConfirmPassword can not be empty!")]
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                //if (value != Password)
                //{
                //    AddError(nameof(ConfirmPassword), "Password can not matched!");
                //}
                if (value != Password)
                {
                    if (!HasErrored(nameof(ConfirmPassword)))
                        AddError(nameof(ConfirmPassword), "Password can not matched!");
                }
                else
                {
                    ClearErrors(nameof(ConfirmPassword));
                    ClearErrors(nameof(Password));
                }
            }
        }
    }
}
