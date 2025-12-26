using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace ToDo.Client.Home.ViewModels
{
    public class AddPriorityViewModel : BindableBase, IDialogAware
    {
        // Dialog window title
        public string Title { get; set; } = string.Empty;
        public DialogCloseListener RequestClose { get; set; }

        // Pop window object
        private readonly ISnackbarService snackbarService;
        public PriorityDTO PriorityDTO { get; set; }

        #region DTO Properties
        private string dtoTitle = string.Empty;
        public string DtoTitle
        {
            get => dtoTitle;
            set
            { SetProperty(ref dtoTitle, value); PriorityDTO.Title = value; }
        }

        private string description = string.Empty;
        public string Description
        {
            get => description;
            set { SetProperty(ref description, value); PriorityDTO.Description = value; }
        }

        private int state = -100;
        public int State
        {
            get => state;
            set { SetProperty(ref state, value); PriorityDTO.State = value; }
        }

        private DateTime dDL = DateTime.Now.AddDays(1);
        public DateTime DDL
        {
            get => dDL;
            set { SetProperty(ref dDL, value); PriorityDTO.DDL = value; }
        }
        #endregion

        public DelegateCommand AddPriorityCommand { get; set; }
        public DelegateCommand<string> SelectLevelCommand { get; set; }

        public AddPriorityViewModel(ISnackbarService snackbarService)
        {
            AddPriorityCommand = new(AddPriority);
            SelectLevelCommand = new(SelectLevel);
            PriorityDTO = new();
            this.snackbarService = snackbarService;
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        /// <summary>
        /// Fill the params when parameters is not null
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {
            PriorityDTO.InsertTime = DateTime.MinValue;
            PriorityDTO.CompletedTime = null;

            if (parameters.Count > 0)
            {
                var param = parameters.GetValue<PriorityModel>("param");
                DtoTitle = param.Title;
                Description = param.Description;
                State = (int)param.State;
                DDL = param.DDL;
            }
        }

        /// <summary>
        /// Change the level of current priority when select the menuitem.
        /// </summary>
        /// <param name="level">The level of current priority </param>
        private void SelectLevel(string level)
        {
            State = level switch
            {
                "normal" => 4,
                "emergency" => 1,
                _ => -100,
            };
        }

        /// <summary>
        /// Add a priority
        /// </summary>
        private void AddPriority()
        {
            // check submit
            if (DtoTitle == string.Empty || State == -100)
            {
                snackbarService.Show("Error", "Can not be empty!",
                    ControlAppearance.Primary,
                    new SymbolIcon(SymbolRegular.AlertOn24),
                    TimeSpan.FromSeconds(2));
                return;
            }

            // Initial DTO
            PriorityDTO.Title = DtoTitle;
            PriorityDTO.Description = Description;
            PriorityDTO.State = State;
            PriorityDTO.DDL = DDL;
            PriorityDTO.InsertTime = DateTime.Now;
            PriorityDTO.CompletedTime = null;

            DialogResult result = new();
            result.Parameters.Add(nameof(PriorityDTO), PriorityDTO);

            // transfer DTO
            RequestClose.Invoke(result);
        }
    }
}
