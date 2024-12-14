namespace Common.Utils
{
    public struct Result<T>
    {
        public readonly T Object;
        public readonly bool IsExist;

        public Result(T result, bool isExist)
        {
            Object = result;
            IsExist = isExist;
        }
        
        public static Result<T> Success(T result)
        {
            return new Result<T>(result, true);
        }

        public static Result<T> Fail()
        {
            return new Result<T>(default, false);
        }
    }
}