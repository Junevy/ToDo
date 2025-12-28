using ToDo.WebAPI.Client;
using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;
using ToDo.WebAPI.Services.Interface;

namespace ToDo.WebAPI.Services
{
    public class HttpService(RequestClient httpClient) : IApi
    {
        private readonly RequestClient httpClient = httpClient;

        public async Task<Response<TResponse>> GetRequestAsync<TRequest, TResponse>(string route)
        {
            var request = new Request<TRequest>()
            {
                Route = route,
                Method = RestSharp.Method.GET,
            };

            return await httpClient.ExecuteAsync<TRequest, TResponse>(request);
        }

        public async Task<Response<TResponse>> PostRequestAsync<TRequest, TResponse>(string route, TRequest dto)
        {
            var request = new Request<TRequest>
            {
                Route = route,
                Method = RestSharp.Method.POST,
                Params = dto
            };

            return await httpClient.ExecuteAsync<TRequest, TResponse>(request);
        }

        public async Task<Response<TResponse>> PutRequestAsync<TRequest, TResponse>(string route, TRequest dto)
        {
            var request = new Request<TRequest>
            {
                Route = route,
                Method = RestSharp.Method.PUT,
                Params = dto
            };

            return await httpClient.ExecuteAsync<TRequest, TResponse>(request);
        }
    }
}
