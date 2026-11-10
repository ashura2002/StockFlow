using Application.Common.Errors;

namespace Application.Common.Results
{
    public class ResultWithValue<T> : Result
    {
        public T? Value { get; }

        protected ResultWithValue(bool isSuccess, T? value, Error? error) : base(isSuccess, error)
        {
            Value = value;
        }

        public static ResultWithValue<T> Success(T value)
        {
            return new ResultWithValue<T>(true, value, null);
        }

        public static new ResultWithValue<T> Failure(Error error)
        {
            return new ResultWithValue<T>(false, default, error);
        }
    }
}
