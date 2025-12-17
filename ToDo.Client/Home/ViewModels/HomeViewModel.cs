using System.Collections.ObjectModel;
using System.Web.UI.WebControls;
using ToDo.Client.Models;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly ISnackbarService snackbarService;
        public ObservableCollection<PriorityModel> Priorities { get; private set; } = [];
        public DelegateCommand<Guid?> ChangeToNormalCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToRemindCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToDiscardCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToEmergencyCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToCompletedCommand { get; set; }
        public DelegateCommand ShowSnackbarCommand { get; set; }

        public HomeViewModel(ISnackbarService snackbarService)
        {

            this.snackbarService = snackbarService;
            
            ChangeToNormalCommand = new(ChangeToNormal);
            ChangeToRemindCommand = new(ChangeToRemind);
            ChangeToDiscardCommand = new(ChangeToDiscard);
            ChangeToEmergencyCommand = new(ChangeToEmergency);
            ChangeToCompletedCommand = new(ChangeToCompleted);
            ShowSnackbarCommand = new(ShowSnackBar);


            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Priority, DateTime.Today, DateTime.Today);
            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Normal, DateTime.Today, DateTime.Today);

        }

        private void ShowSnackBar()
        {
            //var test  = this.snackbarService.GetSnackbarPresenter();
            snackbarService.Show(
                "Importent Notification!",
                "The notification just a simple and test notification.",
                ControlAppearance.Primary,
                new SymbolIcon(SymbolRegular.AlertOn24),
                TimeSpan.FromSeconds(2)
            );
        }

        private void AddProperties(string title, string desciption, PriorityStatus status, DateTime insertTime, DateTime dDL)
        {
            var model = new PriorityModel(title, desciption, status, insertTime, dDL);
            Priorities.Add(model);
        }


        #region change command
        private void ChangeToCompleted(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.Completed);
        }

        private void ChangeToEmergency(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.Priority);
        }

        private void ChangeToDiscard(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.Discarded);
        }

        private void ChangeToRemind(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.RemindTomorrow);
        }

        private void ChangeToNormal(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.Normal);
        }

        #endregion

    }
}
