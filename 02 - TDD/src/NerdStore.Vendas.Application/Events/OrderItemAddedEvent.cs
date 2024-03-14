using MediatR;
using NerdStore.Core;

namespace NerdStore.Vendas.Application.Orders;

public class OrderItemAddedEvent : Event
{
  public Guid CustomerId { get; private set; }
  public Guid ProductId { get; private set; }
  public Guid OrderId { get; private set; }
  public int Quantity { get; private set; }
  public string ProductName { get; private set; }
  public decimal UnitValue { get; private set; }

  public OrderItemAddedEvent(Guid customerId, Guid productId, Guid orderId, string productName, int quantity, decimal unitValue)
  {
    AggregateId = productId;
    CustomerId = customerId;
    OrderId = orderId;
    ProductId = productId;
    Quantity = quantity;
    ProductName = productName;
    UnitValue = unitValue;
  }
}
