using Newtonsoft.Json;
using RestSharp;
using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;

namespace ToDo.WebAPI.HttpClient
{
    public class HttpRequestClient(string routeHead)
    {
        private readonly RestClient client = new();
        private readonly string routeHead = routeHead;

        public async Task<Response<T>> ExecuteAsync<T>(Request<T> request)
        {
            var re = new RestRequest(request.Method);

            // Sets the type of request content 
            re.AddHeader("Content-Type", request.ContentType);

            // Serialize params if it's not null to body
            if (request.Params is not null)
            {
                re.AddJsonBody(request.Params);
            }

            // Sets the url of request
            client.BaseUrl = new Uri(routeHead + request.Route);

            // Get request result
            var res = await client.ExecuteAsync(re);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                // Deserialize params of response to object
                return JsonConvert.DeserializeObject<Response<T>>(res.Content)
                    ?? new Response<T>() { Code = -1, Message = "Server error", Data = default };

            // return default result
            return new Response<T>() { Code = -1, Message = "Server error", Data = default };
        }
    }
}
