using DryIoc.ImTools;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.ObjectModel;
using ToDo.Client.Models;
using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Overview.ViewModels
{
    public class OverviewViewModel
    {
        private readonly HttpService httpService;

        public ObservableCollection<PriorityModel> CompletedList { get; set; } = [];
        public IEnumerable<PriorityStatus> PriorityStatusList => Enum.GetValues(typeof(PriorityStatus)).Cast<PriorityStatus>();

        public AsyncDelegateCommand LoadCompletedPriorityCommand { get; set; }
        public AsyncDelegateCommand<object> UpdatePriorityCommand { get; set; }
        public OverviewViewModel(HttpService httpService)
        {
            this.httpService = httpService;

            LoadCompletedPriorityCommand = new(LoadCompletedPriorityAsync);
            UpdatePriorityCommand = new(UpdatePriorityAsync);
        }

        private async Task UpdatePriorityAsync(object model)
        {
            
        }

        private async Task LoadCompletedPriorityAsync()
        {
            var response = await httpService.GetRequestAsync<MainInfoDTO>("/Priority/GetCompleted");

            if (response != null)
                AddPriorityToList(response.Data.Priorities);
        }

        public void AddPriorityToList(IEnumerable<PriorityDTO> priorities)
        {
            CompletedList.Clear();

            foreach (var p in priorities)
            {
                if (p.State == 0)
                    CompletedList.Add(p.ToModel());
            }

        }
    }
}
