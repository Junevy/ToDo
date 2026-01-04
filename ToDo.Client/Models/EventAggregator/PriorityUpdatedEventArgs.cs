using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Models.EventAggregator
{
    public class PriorityUpdatedEventArgs(Guid id, PriorityDTO dto, string message, DateTime updateTime)
    {
        public Guid Id { get; } = id;
        public PriorityDTO DTO { get; set; } = dto;
        public string Message { get; set; } = message;
        public DateTime UpdateTime { get; set; } = updateTime;

    }
}
