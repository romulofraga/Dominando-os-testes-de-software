namespace NerdStore.Core;

public abstract class Entity
{
  public Guid Id { get; set; }
  public DateTime Timestamp { get; private set; }

  protected Entity()
  {
    Id = Guid.NewGuid();
    Timestamp = DateTime.Now;
  }
}
