using MediatR;
using NerdStore.Core.Messages;

namespace NerdStore.Core;

public class DomainNotification: Message, INotification
{
    public DateTime Timestamp { get; private set; }
    public Guid DomainNotificationId { get; private set; }
    public string DomainNotificationKey { get; private set; }
    public string DomainNotificationValue { get; private set; }
    public int Version { get; set; }

    public DomainNotification(string domainNotificationKey, string domainNotificationValue)
    {
        Timestamp = DateTime.Now;
        DomainNotificationId = Guid.NewGuid();
        DomainNotificationKey = domainNotificationKey;
        DomainNotificationValue = domainNotificationValue;
    }
}
