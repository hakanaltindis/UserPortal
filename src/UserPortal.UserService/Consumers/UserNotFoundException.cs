using System.Runtime.Serialization;

namespace UserPortal.UserService.Consumers
{
  [Serializable]
  public class UserNotFoundException : Exception
  {
    private int Id { get; set; }

    public UserNotFoundException(int id)
      : base($"The user cannot be found with id {id}")
    {
      Id = id;
    }

    public UserNotFoundException()
    {
    }

    public UserNotFoundException(string? message) : base(message)
    {
    }

    public UserNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UserNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}
