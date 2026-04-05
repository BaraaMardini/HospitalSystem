    public class ApiResult<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }

        public ErrorType ErrorType { get; set; } = ErrorType.None;

        public ApiResult() { }

        public ApiResult(T? data, string? message, ErrorType errorType)
        {
            Data = data;
            Message = message;
            ErrorType = errorType;
        }


    }




