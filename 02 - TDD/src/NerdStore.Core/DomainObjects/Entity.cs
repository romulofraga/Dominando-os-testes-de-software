namespace NerdStore.Core;

public abstract class Entity
{
  public Guid Id { get; set; }
  public DateTime Timestamp { get; private set; }

  private List<Event> _notifications;

  public IReadOnlyCollection<Event>? Notifications => _notifications?.AsReadOnly();

  protected Entity()
  {
    Id = Guid.NewGuid();
    Timestamp = DateTime.Now;
  }

  public void AddEvent(Event eventItem)
  {
    _notifications ??= [];
    _notifications.Add(eventItem);
  }

  public void RemoveEvent(Event eventNotification) => _notifications?.Remove(eventNotification);

  public void ClearEvents() => _notifications?.Clear();
}
