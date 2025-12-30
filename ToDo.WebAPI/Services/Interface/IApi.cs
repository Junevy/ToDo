using ToDo.WebAPI.Response;

namespace ToDo.WebAPI.Services.Interface
{
    public interface IApi
    {
        /// <summary>
        /// Http Get Request
        /// </summary>
        /// <typeparam name="TRequest">Request DTO type</typeparam>
        /// <typeparam name="TResponse">Response DTO type</typeparam>
        /// <param name="route">Route</param>
        /// <returns>The result of WebAPI</returns>
        Task<Response<TResponse>> GetRequestAsync<TRequest, TResponse>(string route);
        Task<Response<TResponse>> GetRequestAsync<TResponse>(string route);

        /// <summary>
        /// Http Post Request
        /// </summary>
        /// <typeparam name="TRequest">Request DTO type</typeparam>
        /// <typeparam name="TResponse">Response DTO type</typeparam>
        /// <param name="route">Route</param>
        /// <param name="dto">DTO, to WebAPI</param>
        /// <returns>The result of WebAPI</returns>
        Task<Response<TResponse>> PostRequestAsync<TRequest, TResponse>(string route, TRequest dto);

        /// <summary>
        /// Http Put Request
        /// </summary>
        /// <typeparam name="TRequest">Request DTO type</typeparam>
        /// <typeparam name="TResponse">Response DTO type</typeparam>
        /// <param name="route">Route</param>
        /// <param name="dto">DTO, to WebAPI</param>
        /// <returns>The result of WebAPI</returns>
        Task<Response<TResponse>> PutRequestAsync<TRequest, TResponse>(string route, TRequest dto);
    }
}