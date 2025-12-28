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
        private readonly PriorityApiService apiService;

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
            PriorityApiService apiService,
            NotificationService notify)
        {
            this.snackbarService = snackbarService;
            this.dialogService = dialogService;
            this.apiService = apiService;
            this.notify = notify;

            InfoModel = new();

            //ShowSnackbarCommand = new(ShowSnackBar);
            EditPriorityCommand = new(EditPriority);
            ChangeStatusCommand = new(ChangePriorityStatus);
            RefreshCommand = new(RefreshPriorityAsync);

            ShowAddPriorityCommand = new(() =>
            {
                dialogService.ShowDialog("AddPriorityView", AddPriorityResultCallback);
            });
        }

        /// <summary>
        /// Change the status of priorities
        /// </summary>
        /// <param name="obj">The level of selected item</param>
        private async Task ChangePriorityStatus(object[]? obj)
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

            model.State = state;
            var dto = model.ToDTO();

            if (dto != null)
            {
                await httpService.PutRequestAsync<PriorityDTO>("/Priority/Update", dto);
            }
            await RefreshPriorityAsync();
        }

        /// <summary>
        /// The callback of add priority
        /// </summary>
        /// <param name="result">Priority params</param>
        private async void AddPriorityResultCallback(IDialogResult result)
        {
            try
            {
                var dto = result.Parameters.GetValue<PriorityDTO>(nameof(PriorityDTO));
                if (dto == null)
                    return;

                await AddPriority(dto);
                await RefreshPriorityAsync();
            }
            catch (Exception ex)
            {
                await notify.ShowAsync(TitleType.Error, ex.Message ?? "Add priority error!");
            }
        }

        /// <summary>
        /// Re-fresh priorities from database when add or update and delete priorities after.
        /// </summary>
        private async Task RefreshPriorityAsync()
        {
            try
            {
                var response = await httpService.GetRequestAsync<HomeInfoResponseDTO>("/Priority/Get");

                if (response.Code != 1)
                {
                    await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Refresh Failed!");
                    return;
                }

                UpdatePriorities(response.Data.Priorities);
                OnMainInfoUpdated(response.Data.Priorities);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        /// <summary>
        /// 分类Memos和Priorities
        /// </summary>
        /// <param name="priorities">Memos和Priorities的集合（可枚举对象）</param>
        private void UpdatePriorities(IEnumerable<PriorityDTO> priorities)
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
        /// 当数据刷新后，更新UI
        /// </summary>
        /// <param name="dto"></param>
        private void OnMainInfoUpdated(IEnumerable<PriorityDTO> priorities)
        {
            InfoModel.CompletedCount = priorities.Where(t => t.State == 0)?.Count() ?? 0;
            InfoModel.SummaryCount = priorities?.Count() ?? 0;
            InfoModel.MemosCount = Memos.Count();
        }

        /// <summary>
        /// Change priority and update priority to database.
        /// </summary>
        private async void UpdatePriority(IDialogResult result)
        {

            var dto = result.Parameters.GetValue<PriorityDTO>(nameof(PriorityDTO));
            if (dto != null)
            {
                await httpService.PutRequestAsync<PriorityDTO>("/Priority/Update", dto);
            }

            await RefreshPriorityAsync();
        }

        /// <summary>
        /// Add Priority to database via WebAPI
        /// </summary>
        /// <param name="dto">DTO Model</param>
        /// <returns>The result of add</returns>
        private async Task AddPriority(PriorityDTO dto)
        {
            var response = await httpService.PostRequestAsync<PriorityDTO>("/Priority/Add", dto);

            if (response.Code != 1)
                await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Add Failed!");

            //await notify.ShowMessageAsync("Notification", response.Message ?? "Add Successful!");
        }

        /// <summary>
        /// Command of the Double click priority. Edit the priority
        /// </summary>
        /// <param name="id"></param>
        private void EditPriority(Guid? id)
        {

            var e = Priorities.FirstOrDefault(e => e.Id == id);

            DialogParameters param = new()
            {
                { "param", e }
            };
            dialogService.ShowDialog("AddPriorityView", param, UpdatePriority);
        }

        /// <summary>
        /// Show Pop window when execute the command
        /// </summary>
        private void ShowSnackBar(string title, string message, IconElement? icon = null, int keepSeconds = 2)
        {
            snackbarService.Show(
                title,
                message,
                ControlAppearance.Primary,
                icon ?? new SymbolIcon(SymbolRegular.AlertOn24),
                TimeSpan.FromSeconds(keepSeconds)
            );
        }
    }
}
