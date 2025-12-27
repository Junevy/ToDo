using ToDo.WebAPI.HttpClient;
using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;

namespace ToDo.Client.Services
{
    public class HttpService(HttpRequestClient httpClient)
    {
        private readonly HttpRequestClient httpClient = httpClient;

        public async Task<Response<T>> GetRequestAsync<T>(string route)
        {
            var request = new Request<T>()
            {
                Route = route,
                Method = RestSharp.Method.GET,
            };

            var response = await httpClient.ExecuteAsync<T>(request);
            return response;


        }

        public async Task<Response<T>> PostRequestAsync<T>(string route, T dto)
        {
            var request = new Request<T>
            {
                Route = route,
                Method = RestSharp.Method.POST,
                Params = dto
            };

            var response = await httpClient.ExecuteAsync<T>(request);

            return response;
        }

        public async Task<Response<T>> PutRequestAsync<T>(string route, T dto)
        {
            var request = new Request<T>
            {
                Route = route,
                Method = RestSharp.Method.PUT,
                Params = dto
            };

            return await httpClient.ExecuteAsync<T>(request);

            //return response;
        }
    }
}
