using System.Collections.ObjectModel;
using ToDo.Client.Extensions;
using ToDo.Client.Models;
using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request.DTOs;

namespace ToDo.Client.Overview.ViewModels
{
    public class OverviewViewModel
    {
        private readonly PriorityService apiService;

        public ObservableCollection<PriorityModel> CompletedList { get; set; } = [];
        public ObservableCollection<PriorityModel> QueriedList { get; set; } = [];
        public IEnumerable<PriorityStatus> PriorityStatusList => Enum.GetValues(typeof(PriorityStatus)).Cast<PriorityStatus>();
        public PriorityStatus SelectedStatus { get; set; }

        public QueryByConditionRequestDTO QueryDTO { get; set; }
        public QueryByConditionRequestDTO QueryCompletedDTO { get; set; }

        public AsyncDelegateCommand QueryCompletedPriorityCommand { get; set; }
        public AsyncDelegateCommand<object> UpdatePriorityCommand { get; set; }
        public AsyncDelegateCommand QueryByConditionCommand { get; set; }
        public OverviewViewModel(PriorityService apiService)
        {
            this.apiService = apiService;
            QueryDTO = new();
            QueryCompletedDTO = new()
            {
                Status = 0
            };

            QueryCompletedPriorityCommand = new(QueryCompletedPriorityAsync);
            UpdatePriorityCommand = new(UpdatePriorityAsync);
            QueryByConditionCommand = new(QueryByConditionAsync);
        }

        private async Task QueryByConditionAsync()
        {
            var list = await apiService.QueryByConditionAsync(QueryDTO);
            QueriedList.Clear();

            if (list != null && list.Count > 0)
                AddDtoToList(list, QueriedList);
        }

        private async Task UpdatePriorityAsync(object model)
        {
            if (model is PriorityModel e)
            {
                await apiService.UpdateAsync(e.ToDTO());
            }
        }

        private async Task QueryCompletedPriorityAsync()
        {
            var list = await apiService.QueryByConditionAsync(QueryCompletedDTO);

            if (list != null && list.Count > 0)
                AddDtoToList(list, CompletedList);
        }

        public void AddDtoToList(IEnumerable<PriorityDTO> priorities, ObservableCollection<PriorityModel> obsList)
        {
            foreach (var p in priorities)
            {
                obsList.Add(p.ToModel());
            }
        }
    }
}
