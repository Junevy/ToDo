using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Home.ViewModels
{
    public class AddPriorityViewModel : IDialogAware
    {

        public PriorityDTO PriorityDTO { get; set; }

        public DialogCloseListener RequestClose { get; set; }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
