using System.Collections.ObjectModel;
using ToDo.Client.Models;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ObservableCollection<PriorityModel> Priorities { get; private set; } = [];
        public DelegateCommand<Guid?> ChangeToNormalCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToRemindCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToDiscardCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToEmergencyCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToCompletedCommand { get; set; }
        public DelegateCommand<ValueTuple<string, string>?> ChangeStatusCommand { get; set; }

        public HomeViewModel()
        {
            ChangeToNormalCommand = new(ChangeToNormal);
            ChangeToRemindCommand = new(ChangeToRemind);
            ChangeToDiscardCommand = new(ChangeToDiscard);
            ChangeToEmergencyCommand = new(ChangeToEmergency);
            ChangeToCompletedCommand = new(ChangeToCompleted);

            ChangeStatusCommand = new(ChangeStatus);

            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Priority, TimeSpan.FromDays(22), TimeSpan.FromDays(24));
            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Normal, TimeSpan.FromDays(22), TimeSpan.FromDays(24));

        }

        public void ChangeStatus(ValueTuple<string, string>? tuple)
        {
            //tuple.Item1
        }


        private void AddProperties(string title, string desciption, PriorityStatus status, TimeSpan insertTime, TimeSpan dDL)
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
