using System.ComponentModel.DataAnnotations;
using ToDo.WebAPI.Services;

namespace ToDo.WebAPI.DTOs
{
    public class UserInfoRequestDTO : ValidatableBindableBase
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
            }
        }

        private string confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                SetProperty(ref confirmPassword, value);
                if (value != Password)
                {
                    if (value == string.Empty)
                        ClearErrors(nameof(ConfirmPassword));

                    if (!HasErrored(nameof(ConfirmPassword)))
                        AddError(nameof(ConfirmPassword), "Password can not matched!");
                }
                else
                {
                    ClearErrors(nameof(ConfirmPassword));
                }
            }
        }

        [Required]
        public DateTime SignInDate { get; set; } = DateTime.Now;

        public string Description { get; set; }
    }
}
