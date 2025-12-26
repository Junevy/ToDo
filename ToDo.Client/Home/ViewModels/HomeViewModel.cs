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

        public int CompletedCount { get; private set; }
        public int SummaryCount { get; private set; }
        public string Percentage { get; private set; }
        public int MemosCount { get; private set; }


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

            //model.ReState();
            model.State = state;
            RaisePropertyChanged(nameof(CompletedCount));
            //RaisePropertyChanged(nameof(model));
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

            OnMainInfoUpdated(response.Data);
            UpdatePriorities(response.Data.Priorities);
        }

        private void OnMainInfoUpdated(MainInfoDTO dto)
        {
            CompletedCount = dto.CompletedCount;
            SummaryCount = dto.SummaryCount;
            Percentage = dto.Percentage;
            MemosCount = dto.MemosCount;

            RaisePropertyChanged(nameof(CompletedCount));
            RaisePropertyChanged(nameof(SummaryCount));
            RaisePropertyChanged(nameof(Percentage));
            RaisePropertyChanged(nameof(MemosCount));
        }

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
