namespace UserPortal.Shared
{
  public class Result<T> : Result
  {
    public Result(T obj)
      : base(true)
    {
      Value = obj;
    }
    public Result(string errorMessage)
      : base(errorMessage)
    {
      Value = default;
    }

    public virtual T? Value { get; set; }

    public static Result<T> Ok(T val)
    {
      return new Result<T>(val);
    }
  }

  public class Result
  {
    protected Result(bool isSucceed)
    {
      IsSucceed = isSucceed;
    }

    protected Result(string errorMessage)
    {
      IsSucceed = false;
      ErrorMessage = errorMessage;
    }

    public virtual bool IsSucceed { get; set; }
    public virtual string? ErrorMessage { get; set; } = null;

    public static Result Ok()
    {
      return new Result(true);
    }

    public static Result<T> Ok<T>(T val)
    {
      return new Result<T>(val);
    }

    public static Result Error(string errorMessage)
    {
      return new Result(errorMessage);
    }

    public static Result<T> Error<T>(string errorMessage)
    {
      return new Result<T>(errorMessage);
    }
  }
}
