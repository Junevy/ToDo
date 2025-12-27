using System.Collections.ObjectModel;
using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Overview.ViewModels
{
    public class OverviewViewModel
    {
        public ObservableCollection<PriorityModel> TestList { get; set; } = [];
        public IEnumerable<PriorityStatus> PriorityStatusList { get; set; } = [];
            

        public OverviewViewModel()
        {
            PriorityStatusList = Enum.GetValues(typeof(PriorityStatus)).Cast<PriorityStatus>();

            TestList.Add(new PriorityModel()
            {
                No = 1,
                Title = "test",
                Description = "test",
                InsertTime = DateTime.Now,
                CompletedTime = DateTime.Now,
                State = PriorityStatus.Completed,
                Kind = 0
            });

        }


       
    }
}
