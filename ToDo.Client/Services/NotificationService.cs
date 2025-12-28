namespace ToDo.Client.Services
{
    public class NotificationService
    {
        public async Task ShowAsync(string title, string message)
        {
            var box = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content = message
            };
            await box.ShowDialogAsync();
        }

        public async Task ShowAsync(TitleType title, string message)
        {
            var t = title switch
            {
                TitleType.Notification => "Notification",
                TitleType.Error => "Error",
                _ => "Notification"
            };

            var box = new Wpf.Ui.Controls.MessageBox
            {
                Title = t,
                Content = message
            };
            await box.ShowDialogAsync();
        }


        public void ShowSnackBar(string title, string message)
        {

        }
    }

    public enum TitleType
    {
        Error = 0,
        Notification = 1,
    }
}
