using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToDo.Client.Models
{
    public class PriorityModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; }

        public PriorityModel()
        {
            Id = Guid.NewGuid();
        }
        public PriorityModel
            (string title, string desciption, PriorityStatus status, TimeSpan insertTime, TimeSpan dDL)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = desciption;
            State = status;
            InsertTime = insertTime;
            DDL = dDL;
        }

        private string title;
        public string Title
        {
            get => title;
            private set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private string description;
        public string Description
        {
            get => description;
            private set
            {
                description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));

            }
        }

        private PriorityStatus state;
        public PriorityStatus State
        {
            get => state;
            private set
            {
                state = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        private TimeSpan insertTime;
        public TimeSpan InsertTime
        {
            get => insertTime;
            private set
            {
                insertTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
            }
        }

        private TimeSpan dDL;
        public TimeSpan DDL
        {
            get => dDL;
            private set
            {
                dDL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DDL)));
            }
        }


        public void Rename(string title)
        {
            if (title != null && title != Title)
                Title = title;
        }
        public void ReDescription(string description)
        {
            if (description != null && description != Description)
                Description = description;

        }
        public void ReState(PriorityStatus state)
        {
            if (state != State)
                State = state;
        }
        public void ReInsertTime(TimeSpan insertTime)
        {
            if (insertTime != null && insertTime != InsertTime)
                InsertTime = insertTime;

        }
        public void ReDDL(TimeSpan dDL)
        {
            if (dDL != null && dDL != DDL)
                DDL = dDL;
        }
    }
}
