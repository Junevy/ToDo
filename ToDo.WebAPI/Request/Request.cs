using RestSharp;

namespace ToDo.WebAPI.Request
{
    public class Request<T>
    {
        /// <summary>
        /// The path of request
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Request method of http
        /// </summary>
        public Method Method { get; set; }

        /// <summary>
        /// The parameters of Request
        /// </summary>
        public T Params { get; set; }

        /// <summary>
        /// The type of request
        /// </summary>
        public string ContentType { get; set; } = "application/json";

    }
}
