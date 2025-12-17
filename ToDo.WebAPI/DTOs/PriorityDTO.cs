namespace ToDo.WebAPI.DTOs
{
    public class PriorityDTO
    {
        public string Title { get; set; }
        public string Desciption { get; set; }
        public int State { get; set; }

        public DateTime InsertTime { get; set; }
        public DateTime CompletedTime { get; set; }
        public DateTime DDL { get; set; }

    }
}
