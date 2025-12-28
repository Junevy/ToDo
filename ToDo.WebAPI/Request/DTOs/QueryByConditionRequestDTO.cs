namespace ToDo.WebAPI.Request.DTOs
{
    public class QueryByConditionRequestDTO
    {
        public string PriorityTitle { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Status { get; set; }

    }
}
