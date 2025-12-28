namespace ToDo.WebAPI.Response
{
    public class Response<T>
    {
        /// <summary>
        /// The result code of response
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// The message of response
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The data of response
        /// </summary>
        public T Data { get; set; }

        public bool IsSuccess => Code == 1;

        public static Response<T> Success(T data) =>
            new() { Code = 1, Data = data };

        public static Response<T> Fail(string message) =>
            new() { Code = -1, Message = message };
    }
}
