namespace DatingAppApi.BLL.DTOs
{
    public class ResultDTO<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }

        protected ResultDTO(T value, string error)
        {
            IsSuccess = value != null;
            Value = value;
            Error = error;
        }

        public static ResultDTO<T> Success(T Value) => new ResultDTO<T>(Value, null);
        public static ResultDTO<T> Failure(string Error) => new ResultDTO<T>(default, Error);
    }
}
