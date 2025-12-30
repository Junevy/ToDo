using System.Collections.ObjectModel;
using ToDo.Client.Extensions;
using ToDo.Client.Models;
using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Response.DTOs;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private readonly NotificationService notify;
        private readonly PriorityService priorityService;

        // Pop window object
        private readonly ISnackbarService snackbarService;

        // The priority list
        public ObservableCollection<PriorityModel> Priorities { get; private set; } = [];
        public ObservableCollection<PriorityModel> Memos { get; private set; } = [];

        public HomeInfoModel InfoModel { get; private set; }

        //public DelegateCommand ShowSnackbarCommand { get; set; }
        public AsyncDelegateCommand<object[]?> ChangeStatusCommand { get; set; }
        public DelegateCommand ShowAddPriorityCommand { get; set; }
        public AsyncDelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<Guid?> EditPriorityCommand { get; set; }

        public HomeViewModel(ISnackbarService snackbarService,
            IDialogService dialogService,
            PriorityService priorityService,
            NotificationService notify)
        {
            this.snackbarService = snackbarService;
            this.dialogService = dialogService;
            this.priorityService = priorityService;
            this.notify = notify;

            InfoModel = new();
            _ = UpdateMainUIAsync();


            //ShowSnackbarCommand = new(ShowSnackBar);
            EditPriorityCommand = new(UpdatePriorityDialog);
            ChangeStatusCommand = new(UpdatePriorityStatusAsync);
            RefreshCommand = new(UpdateMainUIAsync);

            ShowAddPriorityCommand = new(() =>
            {
                dialogService.ShowDialog("AddPriorityView", CreatePriorityResultCallbackAsync);
            });
        }

        /// <summary>
        /// Change the status of priority
        /// </summary>
        /// <param name="obj">The level of selected</param>
        private async Task UpdatePriorityStatusAsync(object[]? obj)
        {
            if (obj is not object[] { Length: >= 2 } array ||
                array[0] is not string level ||
                array[1] is not PriorityModel model)
                return;

            var state = level switch
            {
                "Normal" => PriorityStatus.Normal,
                "Priority" => PriorityStatus.Priority,
                "Discard" => PriorityStatus.Discarded,
                "RemindTomorrow" => PriorityStatus.RemindTomorrow,
                "Completed" => PriorityStatus.Completed,
                _ => PriorityStatus.RemindTomorrow,
            };

            var snapStatus = model.State;
            var dto = model.ToDTO();
            dto.State = (int)state;
            var result = await priorityService.UpdatePriorityAsync(dto);

            if (!result)
            {
                model.State = snapStatus;
                await notify.ShowAsync(TitleType.Error, "Update status failed!");
                return;
            }

            if (model.State is PriorityStatus.Completed)
            {
                InfoModel.CompletedCount++;
                await Task.Delay(1000);
                Priorities.Remove(model);
                ShowSnackBar("Successful!", "Good job!");
            }
        }

        #region Dialog Operations
        /// <summary>
        /// Edit the priority command of the Double click priority. 
        /// </summary>
        /// <param name="id"></param>
        private void UpdatePriorityDialog(Guid? id)
        {
            var e = Priorities.FirstOrDefault(e => e.Id == id);

            DialogParameters param = new()
            {
                { "param", e }
            };
            dialogService.ShowDialog("AddPriorityView", param, UpdatePriorityCallBackAsync);
        }

        /// <summary>
        /// Update priority to database When closed the UpdatePriorityDialog dialog
        /// </summary>
        /// <param name="param">Edited priority param</param>
        private void UpdatePriorityCallBackAsync(IDialogResult param)
        {
            var dto = param.Parameters.GetValue<PriorityDTO>(nameof(PriorityDTO));
            if (dto != null)
            {
                _ = priorityService.UpdatePriorityAsync(dto);
            }
        }

        /// <summary>
        /// Re-fresh UI priorities from database
        /// </summary>
        private async Task UpdateMainUIAsync()
        {
            var mainInfo = await priorityService.QueryAllPrioritiesAsync();

            if (mainInfo is null) return;

            var enumer = mainInfo.Priorities.AsEnumerable();
            UpdatePrioritiesList(enumer);
            UpdateHomeInfo(mainInfo.CompletedCount, mainInfo.SummaryCount, mainInfo.MemosCount);
        }


        /// <summary>
        /// The callback of add priority
        /// </summary>
        /// <param name="result">Priority params</param>
        private async void CreatePriorityResultCallbackAsync(IDialogResult param)
        {
            try
            {
                var dto = param.Parameters.GetValue<PriorityDTO>(nameof(PriorityDTO));
                if (dto == null)
                    return;

                await priorityService.CreatePriorityAsync(dto);
            }
            catch (Exception ex)
            {
                await notify.ShowAsync(TitleType.Error, ex.Message ?? "Add priority error!");
            }
        }
        #endregion

        /// <summary>
        /// 当数据刷新后，更新UI
        /// </summary>
        /// <param name="dto"></param>
        private void UpdateHomeInfo(int completedCount, int summaryCount, int memosCount)
        {
            InfoModel.CompletedCount = completedCount;
            InfoModel.SummaryCount = summaryCount;
            InfoModel.MemosCount = memosCount;
        }

        /// <summary>
        /// Pessimistic update, insure the data absolutely keep the same.
        /// </summary>
        /// <param name="list">Priorities list</param>
        /// <param name="dto">The priority that need to be updated</param>
        private void PessimisticUpdate(ObservableCollection<PriorityModel> list, PriorityDTO dto)
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

        /// <summary>
        /// Update Memos and Priorities when refresh the priority
        /// </summary>
        /// <param name="priorities">Memos和Priorities的集合（可枚举对象）</param>
        private void UpdatePrioritiesList(IEnumerable<PriorityDTO> priorities)
        {
            Priorities.Clear();
            Memos.Clear();

            foreach (var p in priorities)
            {
                if (p.Kind != 0)
                {
                    Memos.Add(p.ToModel());
                    continue;
                }

                if (p.State != 0)
                    Priorities.Add(p.ToModel());
            }
        }

        /// <summary>
        /// Show Pop window when execute the command
        /// </summary>
        private void ShowSnackBar(string title, string message, 
            ControlAppearance apprance = ControlAppearance.Primary,
            IconElement? icon = null, 
            int keepSeconds = 2)
        {
            snackbarService.Show(
                title,
                message,
                apprance,
                icon ?? new SymbolIcon(SymbolRegular.AlertOn24),
                TimeSpan.FromSeconds(keepSeconds)
            );
        }
    }
}
