namespace ToDo.Client.Models
{
    public class PriorityModel : BindableBase
    {
        public static int Id { get; set; } = 0;

        public PriorityModel()
        {
            ChangeCompleted = new DelegateCommand<object>(ChangeToCompleted);

            Id += 1;
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);
            }
        }

        private PriorityStatus state;
        public PriorityStatus State
        {
            get => state;
            set
            {
                SetProperty(ref state, value);
            }
        }
        public DelegateCommand<object> ChangeCompleted { get; set; }


        private void ChangeToCompleted(object itemIndex)
        {
            var test = (int)itemIndex;
        }

    }
}
