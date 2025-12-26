namespace ToDo.Client.Models
{
    public class PriorityModel
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
            set
            {
                if (value != null && title != value)
                    title = value;
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                if (value != null && description != value)
                    description = value;

            }
        }

        private PriorityStatus state;
        public PriorityStatus State
        {
            get => state;
            set
            {
                if (state != value)
                    state = value;
            }
        }

        private DateTime insertTime;
        public DateTime InsertTime
        {
            get => insertTime;
            set
            {
                if (value != null && insertTime != value)
                    insertTime = value;
            }
        }

        private DateTime dDL = DateTime.Now;
        public DateTime DDL
        {
            get => dDL;
            set
            {
                if (value != null && dDL != value)
                    dDL = value;
            }
        }

        private DateTime? completedTime;
        public DateTime? CompletedTime
        {
            get => completedTime;
            set
            {
                if (value != null && completedTime != value)
                    completedTime = value;
            }
        }

        private int kind;
        public int Kind
        {
            get => kind;
            set
            {
                if (kind != value)
                    kind = value;
            }
        }
    }
}
