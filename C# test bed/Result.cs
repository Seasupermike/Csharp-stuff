namespace Result
{
	public class TestClass
	{
        public static void Test()
        {
            
        }

    }

	public class Result<TReturn, TError> where TError : Exception
	{
		public TReturn Value;
        public readonly TError? Error;

        private Result(TReturn value, TError? error)
		{
			Value = value;
			Error = error;
		}

		public Result<TReturn, TError> TryThrowError()
		{
			if (Error is not null)
			{
				throw Error;
			}
			return this;
		}

		public TReturn TryGetResult()
		{
            if (Error is not null)
            {
                throw Error;
            }
            return Value;
        }

        public static Result<TReturn, TError> Pass(TReturn value)
        {
            return new Result<TReturn, TError>(value, null);
        }

        public static Result<TReturn, TError> Fail(TError error) 
        {
            return new Result<TReturn, TError>(default, error);
        } 

        public static Result<TReturn, TError> CallbackToResult(Func<TReturn> callback) 
        {
            try
            {
                return Result<TReturn, TError>.Pass(callback());
            } catch (TError error)
            {
                return Result<TReturn, TError>.Fail(error);
            }
        }

        public static Result<TReturn, TError> CallbackToResult(Func<Result<TReturn, TError>> callback)
        {
            return callback();
        }
    }
}
