using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Result
{
	public class TestClass
	{
        public static void Test()
        {
            //Result<float, FormatException> result  
        }

    }

	public class Result<TReturn, TError> where TError : Exception
	{
		public TReturn? Value;
        public readonly TError? Error;

        private Result(TReturn? value, TError? error)
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

		public TReturn? TryGetResult()
		{
            if (Error is null)
            {
                return Value;
            }
            throw Error;
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
    }
}
