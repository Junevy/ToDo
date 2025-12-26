using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;

namespace ToDo.Client.Services
{
    public static class PriorityExtension
    {
        public static PriorityModel ToModel(this PriorityDTO dto)
        {
            return new PriorityModel
            {
                Title = dto.Title,
                State = (PriorityStatus)dto.State,
                Description = dto.Description,
                InsertTime = dto.InsertTime,
                DDL = dto.DDL,
                CompletedTime = dto.CompletedTime,
                Kind = dto.Kind
            };
        }
    }
}
