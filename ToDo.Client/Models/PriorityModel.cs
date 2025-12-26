namespace ToDo.Client.Models
{
    public class PriorityModel : BindableBase
    {
        public Guid Id { get; }

        public PriorityModel()
        {
            Id = Guid.NewGuid();
        }
        public PriorityModel
            (string title, string desciption, PriorityStatus status, DateTime insertTime, DateTime dDL, DateTime completedTime)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = desciption;
            State = status;
            InsertTime = insertTime;
            DDL = dDL;
            CompletedTime = completedTime;
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        private PriorityStatus state;
        public PriorityStatus State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        private DateTime insertTime;
        public DateTime InsertTime
        {
            get => insertTime;
            set => SetProperty(ref insertTime, value);
        }

        private DateTime dDL = DateTime.Now;
        public DateTime DDL
        {
            get => dDL;
            set => SetProperty(ref dDL, value);
        }

        private DateTime? completedTime;
        public DateTime? CompletedTime
        {
            get => completedTime;
            set => SetProperty(ref completedTime, value);
        }

        private int kind;
        public int Kind
        {
            get => kind;
            set => SetProperty(ref kind, value);
        }
    }
}
