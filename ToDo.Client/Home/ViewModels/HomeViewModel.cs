using System.Collections.ObjectModel;
using ToDo.Client.Models;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ObservableCollection<PriorityModel> Priorities { get; set; } = [];

        public HomeViewModel()
        {
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Priority
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Completed
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Discarded
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.RemindTomorrow
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Priority
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Priority
            });
            Priorities.Add(new PriorityModel()
            {
                Title = "Fix bugs",
                Description = "Fix the ToDo applicaation function of navigation.",
                State = PriorityStatus.Priority
            });

            ChangeCompleted = new DelegateCommand<object>(ChangeToCompleted);
        }


        private void ChangeToCompleted(object itemIndex)
        {
            var test = (int)itemIndex;
        }

        public DelegateCommand<object> ChangeCompleted { get; set; }
    }
}
