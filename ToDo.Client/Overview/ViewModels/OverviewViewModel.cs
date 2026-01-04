using System.Collections.ObjectModel;
using ToDo.Client.Extensions;
using ToDo.Client.Models;
using ToDo.Client.Models.EventAggregator;
using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request.DTOs;

namespace ToDo.Client.Overview.ViewModels
{
    public class OverviewViewModel
    {
        private readonly PriorityService apiService;
        private readonly IEventAggregator aggregator;
        private readonly Guid vmId = Guid.NewGuid();
        private PriorityModel rollbackModel;

        public ObservableCollection<PriorityModel> CompletedList { get; set; } = [];
        public ObservableCollection<PriorityModel> QueriedList { get; set; } = [];
        public IEnumerable<PriorityStatus> PriorityStatusList => Enum.GetValues(typeof(PriorityStatus)).Cast<PriorityStatus>();
        public PriorityStatus SelectedStatus { get; set; }

        public QueryByConditionRequestDTO QueryDTO { get; set; }
        public QueryByConditionRequestDTO QueryCompletedDTO { get; set; }

        public AsyncDelegateCommand QueryCompletedPriorityCommand { get; set; }
        // Invoke command.
        public AsyncDelegateCommand<object> UpdatePriorityCommand { get; set; }
        public DelegateCommand<object> UpdateSelectedItemCommand { get; set; }
        public AsyncDelegateCommand QueryByConditionCommand { get; set; }
        public OverviewViewModel(PriorityService apiService, IEventAggregator aggregator)
        {
            this.apiService = apiService;
            this.aggregator = aggregator;
            QueryDTO = new();
            QueryCompletedDTO = new()
            {
                Status = 0
            };

            QueryCompletedPriorityCommand = new(QueryCompletedPriorityAsync);
            UpdatePriorityCommand = new(UpdatePriorityAsync);
            QueryByConditionCommand = new(QueryByConditionAsync);
            UpdateSelectedItemCommand = new(UpdateSelectedItem);

            aggregator.GetEvent<PriorityUpdatedEvent>().Subscribe( (args) =>
            {
                if (args.Id == vmId) return;

                // Update the local list when other ViewModel update the data.
                RollbackPriority(QueriedList, args.DTO);
            });
        }

        /// <summary>
        /// When the data grid item has been mouse or other selected,
        /// Update the <see cref="rollbackModel"/> used to rollback.
        /// Rollback item If request webapi error or failed.
        /// </summary>
        /// <param name="model">Edit the previous model</param>
        private void UpdateSelectedItem(object model)
        {
            if (model is PriorityModel e)
            {
                rollbackModel = e;
            }
        }

        private async Task QueryByConditionAsync()
        {
            var list = await apiService.QueryByConditionAsync(QueryDTO);
            QueriedList.Clear();

            if (list != null && list.Count > 0)
                AddDtoToList(list, QueriedList);
        }

        /// <summary>
        /// Update the priority to database
        /// When request webapi failed, rollback the priority(model): <see cref="PriorityModel"/>
        /// </summary>
        /// <param name="model">Instance of <see cref="PriorityModel"/> by edited</param>
        /// <returns><c>NULL</c></returns>
        private async Task UpdatePriorityAsync(object model)
        {
            PriorityDTO dto;

            if (model is PriorityModel e)
            {
                dto = e.ToDTO();
                var result = await apiService.UpdateAsync(dto);

                if (!result)
                {
                    // Rollback model when request webapi error or failed
                    RollbackPriority(QueriedList, dto);
                    return;
                }

                aggregator.GetEvent<PriorityUpdatedEvent>().Publish(new PriorityUpdatedEventArgs(vmId, dto, "Updated", DateTime.Now));
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

        /// <summary>
        /// Used to Pessimistic Update or rollback.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="dto"></param>
        private void RollbackPriority(ObservableCollection<PriorityModel> list, PriorityDTO dto)
        {
            var temp = list.FirstOrDefault(e => e.No == dto.No);
            if (temp is null) return;

            temp.Title = dto.Title;
            temp.Description = dto.Description;
            temp.State = (PriorityStatus)dto.State;
            temp.DDL = dto.DDL;
            if (temp.CompletedTime is not null)
                temp.CompletedTime = dto.CompletedTime ?? null;
            temp.Kind = dto.Kind;
        }
    }
}
