using ToDo.Client.Models;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request.DTOs;
using ToDo.WebAPI.Response.DTOs;
using ToDo.WebAPI.Services.Interface;

namespace ToDo.Client.Services
{
    public class PriorityService(IApi apiService)
    {
        private readonly IApi apiService = apiService;

        private readonly string createRoute = "/Priority/Create";
        private readonly string queryRoute = "/Priority/Query";
        private readonly string updateRoute = "/Priority/Update";
        private readonly string queryCompletedRoute = "/Priority/QueryCompleted";
        private readonly string queryByConditionRoute = "/Priority/QueryByCondition";


        #region CRUD with Web Api
        public async Task<bool> CreateAsync(PriorityDTO dto)
        {
            var response = await apiService.PostRequestAsync<PriorityDTO, PriorityDTO>(createRoute, dto);
            return response.IsSuccess;
        }

        public async Task<bool> UpdateAsync(PriorityDTO dto)
        {
            var response = await apiService.PutRequestAsync<PriorityDTO, PriorityDTO>(updateRoute, dto);
            return response.IsSuccess;
        }

        /// <summary>
        /// Query TRequestType.
        /// </summary>
        /// <returns>All</returns>
        public async Task<TRequestType> QueryAsync<TRequestType>()
        {
            var response = await apiService.GetRequestAsync<TRequestType>(queryRoute);
            return response.Data;
        }

        public async Task<List<PriorityDTO>> QueryByConditionAsync(QueryByConditionRequestDTO dto)
        {
            var response = await apiService
                .PostRequestAsync<QueryByConditionRequestDTO, List<PriorityDTO>>(queryByConditionRoute, dto);
            return response.Data;
        }
        #endregion

        /// <summary>
        /// Update priority to datebase.
        /// </summary>
        /// <param name="dto">The object that need to update </param>
        /// <returns><see cref="bool"/>, indicates the result of update</returns>
        public async Task<bool> UpdatePriorityAsync(PriorityDTO dto)
        {
            if (dto != null)
            {
                return await UpdateAsync(dto);
            }
            return false;
        }

        /// <summary>
        /// Update priority to datebase.
        /// </summary>
        /// <param name="dto">The object that need to update </param>
        /// <returns><see cref="bool"/>, indicates the result of update</returns>
        public async Task<PriorityDTO> UpdatePriorityAsync(PriorityDTO dto, string newStatus, int snapStatus)
        {
            var status = newStatus switch
            {
                "Normal" => PriorityStatus.Normal,
                "Priority" => PriorityStatus.Priority,
                "Discard" => PriorityStatus.Discarded,
                "RemindTomorrow" => PriorityStatus.RemindTomorrow,
                "Completed" => PriorityStatus.Completed,
                _ => PriorityStatus.RemindTomorrow,
            };

            dto.State = (int)status;
            var result = await UpdatePriorityAsync(dto);

            if (!result)
            {
                dto.State = snapStatus; //失败回滚
            }
            return dto;
        }

        /// <summary>
        /// Add Priority to database via WebAPI
        /// </summary>
        /// <param name="dto">DTO Model</param>
        /// <returns>The result of add</returns>
        public async Task<bool> CreatePriorityAsync(PriorityDTO dto)
        {
            return await CreateAsync(dto);
        }

        /// <summary>
        /// Query all priorities and home info.
        /// </summary>
        /// <returns>All home info and priorities</returns>
        public async Task<HomeInfoResponseDTO>? QueryAllPrioritiesAsync()
        {
            return await QueryAsync<HomeInfoResponseDTO>();
        }
    }
}
