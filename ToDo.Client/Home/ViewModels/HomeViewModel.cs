using System.Collections.ObjectModel;
using System.Windows;
using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.HttpClient;
using ToDo.WebAPI.Request;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly IDialogService dialogService;
        private readonly HttpRequestClient<PriorityDTO> httpClient;

        // Pop window object
        private readonly ISnackbarService snackbarService;

        // The priority list
        public ObservableCollection<PriorityModel> Priorities { get; private set; } = [];

        public int CompletedCount => Priorities.Count(e => (e.State == PriorityStatus.Completed));

        #region change command
        public DelegateCommand<Guid?> ChangeToNormalCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToRemindCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToDiscardCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToEmergencyCommand { get; set; }
        public DelegateCommand<Guid?> ChangeToCompletedCommand { get; set; }
        #endregion
        public DelegateCommand ShowSnackbarCommand { get; set; }
        public DelegateCommand ShowAddPriorityCommand { get; set; }
        public DelegateCommand<Guid?> EditPriorityCommand { get; set; }

        public HomeViewModel(ISnackbarService snackbarService, IDialogService dialogService, HttpRequestClient<PriorityDTO> client)
        {

            this.snackbarService = snackbarService;
            this.dialogService = dialogService;
            this.httpClient = client;

            ChangeToNormalCommand = new(ChangeToNormal);
            ChangeToRemindCommand = new(ChangeToRemind);
            ChangeToDiscardCommand = new(ChangeToDiscard);
            ChangeToEmergencyCommand = new(ChangeToEmergency);
            ChangeToCompletedCommand = new(ChangeToCompleted);
            ShowSnackbarCommand = new(ShowSnackBar);
            EditPriorityCommand = new(EditPriority);

            ShowAddPriorityCommand = new(() =>
            {
                dialogService.ShowDialog("AddPriorityView", AddPriorityResultCallback);
            });
            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Priority, DateTime.Today, DateTime.Today, DateTime.Today);
            AddProperties("Bug fix", "Fix the ui bugs when today.", PriorityStatus.Normal, DateTime.Today, DateTime.Today, DateTime.Today);

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
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        System.Windows.MessageBox.Show(t.Exception.InnerException?.Message ?? "Save error!");
                    });
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdatePriority();
                });
            }, TaskScheduler.Default);
        }

        private void UpdatePriority()
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
            var request = new Request<PriorityDTO>()
            {
                Route = "/Priority/Add",
                Method = RestSharp.Method.POST,
                Params = dto
            };

            var response = await httpClient.ExecuteAsync(request);

            if (response.Code != 1)
            {
                var updateError = new Wpf.Ui.Controls.MessageBox
                {
                    Title = "Error",
                    Content = response.Message ?? "Sign in error!"
                };
                await updateError.ShowDialogAsync();
                return;
            }

            var updateScs = new Wpf.Ui.Controls.MessageBox
            {
                Title = "Notification",
                Content = response.Message
            };
            await updateScs.ShowDialogAsync();
        }

        private void EditPriority(Guid? id)
        {

            var e = Priorities.FirstOrDefault(e => e.Id == id);

            DialogParameters param = new()
            {
                { "param", e }
            };
            dialogService.ShowDialog("AddPriorityView", param);
        }

        /// <summary>
        /// Show Pop window when execute the command
        /// </summary>
        private void ShowSnackBar()
        {
            snackbarService.Show(
                "Importent Notification!",
                "The notification just a simple and test notification.",
                ControlAppearance.Primary,
                new SymbolIcon(SymbolRegular.AlertOn24),
                TimeSpan.FromSeconds(2)
            );
        }

        /// <summary>
        /// Add the Priority function
        /// </summary>
        /// <param name="title">Title </param>
        /// <param name="desciption">Descripton</param>
        /// <param name="status">The level of priority</param>
        /// <param name="insertTime">The time of add</param>
        /// <param name="dDL">The time of except completed</param>
        private void AddProperties(string title, string desciption, PriorityStatus status, DateTime insertTime, DateTime dDL, DateTime completedTime)
        {
            var model = new PriorityModel(title, desciption, status, insertTime, dDL, completedTime);
            Priorities.Add(model);
        }


        #region change command
        private void ChangeToCompleted(Guid? gUid)
        {
            Priorities
                .FirstOrDefault(e => e.Id == gUid)
                ?.ReState(PriorityStatus.Completed);
            RaisePropertyChanged(nameof(CompletedCount));
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
