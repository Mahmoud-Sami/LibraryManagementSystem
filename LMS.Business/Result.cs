namespace LMS.Business
{
    public abstract class ResultBase
    {
        public bool Success { get; private set; }

        public bool Failed { get => !Success; }

        public string Message { get; init; }

        protected ResultBase(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
    }

    public class Result : ResultBase
    {
        protected Result(bool success, string message)
            : base(success, message)
        {
        }

        public static Result Ok(string message = "Success") => new Result(true, message);
        public static Result Error(string message) => new Result(false, message);

        public static Result<T> Ok<T>(T value, string message = "Success") where T : class 
            => new Result<T>(true, message, value);
    }

    public class Result<T> : ResultBase
    {
        internal Result(bool success, string message, T data)
            : base(success, message)
        {
            Data = data;
        }

        public T Data { get; init; }

        public static implicit operator Result<T>(Result result)
            => new Result<T>(result.Success, result.Message, default!);
    }
}
