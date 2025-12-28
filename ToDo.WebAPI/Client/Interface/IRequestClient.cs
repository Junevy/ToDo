using ToDo.WebAPI.Request;
using ToDo.WebAPI.Response;

namespace ToDo.WebAPI.Client.Interface
{
    public interface IRequestClient
    {
        Task<Response<TResponse>> ExecuteAsync<TRequest, TResponse>(Request<TRequest> request);
    }
}