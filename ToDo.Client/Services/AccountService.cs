using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Services.Interface;

namespace ToDo.Client.Services
{
    public class AccountService(IApi apiService)
    {
        private readonly IApi apiService = apiService;
        private readonly string loginRoute = "/Users/Login";
        private readonly string registerRoute = "/Users/Register";

        public async Task<bool> Login(string userName, string password)
        {
            // 创建请求，设定 路由、请求方式、DTO
            var response = await apiService.GetRequestAsync<UserInfoRequestDTO, bool>(loginRoute + $"?account={userName}&password={password}");
            return response.IsSuccess;
        }

        public async Task<bool> Regeister(UserInfoRequestDTO dto)
        {
            // 创建请求，设定 路由、请求方式、DTO
            var response = await apiService.PostRequestAsync<UserInfoRequestDTO, bool>(registerRoute, dto);
            return response.IsSuccess;
        }
    }
}
