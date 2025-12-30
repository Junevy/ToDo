using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request.DTOs;
using ToDo.WebAPI.Services.Interface;

namespace ToDo.Client.Services
{
    public class PriorityApiService(IApi apiService)
    {
        private readonly IApi apiService = apiService;

        private readonly string createRoute = "/Priority/Create";
        private readonly string queryRoute = "/Priority/Query";
        private readonly string updateRoute = "/Priority/Update";
        private readonly string queryCompletedRoute = "/Priority/QueryCompleted";
        private readonly string queryByConditionRoute = "/Priority/QueryByCondition";


        public async Task<bool> Create(PriorityDTO dto)
        {
            var response = await apiService.PostRequestAsync<PriorityDTO, PriorityDTO>(createRoute, dto);
            return response.IsSuccess;
        }

        public async Task<bool> Update(PriorityDTO dto)
        {
            var response = await apiService.PutRequestAsync<PriorityDTO, PriorityDTO>(updateRoute, dto);
            return response.IsSuccess;
        }

        /// <summary>
        /// Query TRequestType.
        /// </summary>
        /// <returns>All</returns>
        public async Task<TRequestType> Query<TRequestType>()
        {
            var response = await apiService.GetRequestAsync<TRequestType>(queryRoute);
            return response.Data;
        }

        public async Task<List<PriorityDTO>> QueryByCondition(QueryByConditionRequestDTO dto)
        {
            var response = await apiService
                .PostRequestAsync<QueryByConditionRequestDTO, List<PriorityDTO>>(queryByConditionRoute, dto);
            return response.Data;
        }
    }
}
