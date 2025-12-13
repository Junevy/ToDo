namespace ToDo.WebAPI.Response
{
    public class AccountResponse<T>
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
    }
}
