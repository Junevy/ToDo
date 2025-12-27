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
                No = dto.No,
                Title = dto.Title,
                State = (PriorityStatus)dto.State,
                Description = dto.Description,
                InsertTime = dto.InsertTime,
                DDL = dto.DDL,
                CompletedTime = dto.CompletedTime,
                Kind = dto.Kind
            };
        }

        public static PriorityDTO ToDTO(this PriorityModel model)
        {
            return new PriorityDTO
            {
                No = model.No,
                Title = model.Title,
                State = (int)model.State,
                Description = model.Description,
                InsertTime = model.InsertTime,
                DDL = model.DDL,
                CompletedTime = model.CompletedTime,
                Kind = model.Kind
            };
        }
    }
}
