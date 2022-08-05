namespace UserPortal.Events
{
  public class EventBase
  {
    public string SourceProvider { get; set; } = string.Empty;
    public string SourceKey { get; set; } = string.Empty;
  }
}
