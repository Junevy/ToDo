using System.ComponentModel.DataAnnotations;

namespace ToDo.WebAPI.DTOs
{
    public class PriorityDTO
    {
        public int No { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int State { get; set; } = -100;

        public DateTime InsertTime { get; set; } = DateTime.Now;
        public DateTime? CompletedTime { get; set; } = null;
        public DateTime DDL { get; set; } = DateTime.Now.AddDays(1);
        public int Kind { get; set; }
    }
}
