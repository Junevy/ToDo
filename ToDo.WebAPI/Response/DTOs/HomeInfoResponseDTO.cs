using ToDo.WebAPI.DTOs;

namespace ToDo.WebAPI.Response.DTOs
{
    public class HomeInfoResponseDTO
    {
        public int SummaryCount { get; set; }

        public int CompletedCount { get; set; }

        public string Percentage
        {
            get
            {
                if (SummaryCount == 0) return "100%";
                return (CompletedCount * 100 / SummaryCount).ToString("f2") + "%";
            }
        }

        public int MemosCount { get; set; }

        public List<PriorityDTO> Priorities { get; set; }
    }
}
