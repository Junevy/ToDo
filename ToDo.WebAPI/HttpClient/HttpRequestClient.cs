using Newtonsoft.Json;
using RestSharp;
using ToDo.WebAPI.DTOs;
using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;

namespace ToDo.WebAPI.HttpClient
{
    public class HttpRequestClient
    {
        private readonly RestClient client;
        private readonly string routeHead = "http://localhost:6338/api";

        public HttpRequestClient()
        {
            client = new();
        }

        public async Task<AccountResponse<AccountDTO>> ExecuteAsync(AccountRequest<AccountDTO> request)
        {
            var re = new RestRequest(request.Method);

            // Sets the type of request content 
            re.AddHeader("Content-Type", request.ContentType);

            // Serialize params if it's not null to body
            if (request.Params is not null)
            {
                re.AddJsonBody(request.Params);
                //var jsonParams = JsonConvert.SerializeObject(request.Params);
                //re.AddParameter("param", jsonParams);
            }

            // Sets the url of request
            client.BaseUrl = new Uri(routeHead + request.Route);

            // Get request result
            var res = await client.ExecuteAsync(re);

            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                // Deserialize params of response to object
                return JsonConvert.DeserializeObject<AccountResponse<AccountDTO>>(res.Content)
                    ?? new AccountResponse<AccountDTO>() { Code = -1, Message = "Server error", Data = null };

            // return default result
            return new AccountResponse<AccountDTO>() { Code = -2, Message = "Server error", Data = null };
        }
    }
}
