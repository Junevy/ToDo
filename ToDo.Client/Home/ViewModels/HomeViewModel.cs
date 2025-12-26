using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using ToDo.Client.Models;
using ToDo.Client.Services;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.HttpClient;
using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private readonly NotificationService notify;
        private readonly HttpService httpService;

        // Pop window object
        private readonly ISnackbarService snackbarService;

        // The priority list
        public ObservableCollection<PriorityModel> Priorities { get; private set; } = [];
        public ObservableCollection<PriorityModel> Memos { get; private set; } = [];

        public MainInfoModel InfoModel { get; private set; }

        public int CompletedCount => Priorities.Where(t => t.State == 0).Count();
        public int SummaryCount => Priorities.Count() + Memos.Count();
        public string Percentage => (CompletedCount * 100 / (SummaryCount == 0 ? 1 : SummaryCount)).ToString("f2") + "%";
        public int MemosCount => Memos.Count();


        //public DelegateCommand ShowSnackbarCommand { get; set; }
        public DelegateCommand<object[]?> ChangeStatusCommand { get; set; }
        public DelegateCommand ShowAddPriorityCommand { get; set; }
        public AsyncDelegateCommand RefreshCommand { get; set; }
        public DelegateCommand<Guid?> EditPriorityCommand { get; set; }

        public HomeViewModel(ISnackbarService snackbarService,
            IDialogService dialogService,
            HttpService httpService,
            NotificationService notify)
        {
            this.snackbarService = snackbarService;
            this.dialogService = dialogService;
            this.httpService = httpService;
            this.notify = notify;

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
        private void ChangePriorityStatus(object[]? obj)
        {
            var level = ((string)obj[0]) ?? "RemindTomorrow";
            var model = (PriorityModel)obj[1];

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
            if (model.State == PriorityStatus.Completed)
            {
                Priorities.Remove(model);
            }


            var test  = Priorities.OrderBy(t => t.State).ToList();
            Priorities.Clear();
            Priorities.AddRange(test);

            
            OnMainInfoUpdated();
        }

        /// <summary>
        /// The callback of add priority
        /// </summary>
        /// <param name="result">Priority params</param>
        private void AddPriorityResultCallback(IDialogResult result)
        {
            var dto = result.Parameters.GetValue<PriorityDTO>(nameof(PriorityDTO));

            _ = AddPriority(dto).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(async () =>
                    {
                        await notify.ShowMessageAsync(TitleType.Error, t.Exception.InnerException?.Message ?? "Sign in error!");
                        return;
                    });
                }

                Application.Current.Dispatcher.BeginInvoke(async () =>
                {
                    await RefreshPriorityAsync();
                });
            }, TaskScheduler.Default);
        }

        /// <summary>
        /// Re-fresh priorities from database when add or update and delete priorities after.
        /// </summary>
        private async Task RefreshPriorityAsync()
        {
            var response = await httpService.GetRequestAsync<MainInfoDTO>("/Priority/Get");

            if (response.Code != 1)
            {
                await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Refresh Failed!");
                return;
            }

            UpdatePriorities(response.Data.Priorities);
            OnMainInfoUpdated();

        }

        /// <summary>
        /// 当数据刷新后，更新UI
        /// </summary>
        /// <param name="dto"></param>
        private void OnMainInfoUpdated()
        {

            RaisePropertyChanged(nameof(CompletedCount));
            RaisePropertyChanged(nameof(SummaryCount));
            RaisePropertyChanged(nameof(Percentage));
            RaisePropertyChanged(nameof(MemosCount));
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
                Priorities.Add(p.ToModel());
            }
        }

        /// <summary>
        /// Change priority and update priority to database.
        /// </summary>
        private void UpdatePriority(IDialogResult result)
        {
            var request = new Request<PriorityDTO>();
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
            {
                await notify.ShowMessageAsync(TitleType.Error, response.Message ?? "Add Failed!");
                return;
            }

            await notify.ShowMessageAsync("Notification", response.Message ?? "Add Successful!");
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
