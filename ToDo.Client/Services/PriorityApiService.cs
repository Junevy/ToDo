using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request.DTOs;
using ToDo.WebAPI.Services.Interface;

namespace ToDo.Client.Services
{
    public class PriorityApiService(IApi apiService)
    {
        private readonly IApi apiService = apiService;

        private readonly string createRoute = "/Priority/Query";
        private readonly string queryRoute = "/Priority/Query";
        private readonly string updateRoute = "/Priority/Update";
        private readonly string queryCompletedRoute = "/Priority/QueryCompleted";
        private readonly string queryByConditionRoute = "/Priority/QueryByCondition";

        public async Task<List<PriorityDTO>> QueryByCondition(QueryByConditionRequestDTO dto)
        {
            var response = await apiService
                .PostRequestAsync<QueryByConditionRequestDTO, List<PriorityDTO>>(queryByConditionRoute, dto);
            return response.Data;
        }

        public async Task<bool> Update(PriorityDTO dto)
        {
            var response = await apiService.PutRequestAsync<PriorityDTO, string>(updateRoute, dto);
            return response.IsSuccess;
        }

        public async Task<bool> Create(PriorityDTO dto)
        {
            var response = await apiService.PostRequestAsync<PriorityDTO, string>(createRoute, dto);
            return response.IsSuccess;
        }
    }
}
