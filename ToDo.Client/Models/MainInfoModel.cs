namespace ToDo.Client.Models
{
    public class MainInfoModel : BindableBase
    {
        private int summaryCount;
        public int SummaryCount
        {
            get => summaryCount;
            set { SetProperty(ref summaryCount, value); RaisePropertyChanged(nameof(Percentage)); }
        }

        private int completedCount;
        public int CompletedCount
        {
            get => completedCount;
            set { SetProperty(ref completedCount, value); RaisePropertyChanged(nameof(Percentage)); }
        }

        public string Percentage
        {
            get
            {
                if (SummaryCount == 0) return "100%";
                return (CompletedCount * 100.0 / Math.Max(SummaryCount, 1)).ToString("f2") + "%";
            }
        }

        private int memosCount;
        public int MemosCount { get => memosCount; set => SetProperty(ref memosCount, value); }

    }
}
