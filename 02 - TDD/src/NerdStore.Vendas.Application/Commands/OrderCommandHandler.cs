
using MediatR;
using NerdStore.Core;
using NerdStore.Sales.Domain;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Orders;

public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
{
  private readonly IOrderRepository _pedidoRepository;
  private readonly IMediator _mediator;

  public OrderCommandHandler(IOrderRepository pedidoRepository, IMediator mediator)
  {
    _pedidoRepository = pedidoRepository;
    _mediator = mediator;
  }

  public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
  {
    if(!ValidateCommand(message)) return false;

    var order = await _pedidoRepository.GetDraftOrderByClientId(message.CustomerId);
    var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnityValue);

    if (order is null)
    {
      order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
      order.AddItem(orderItem);
      _pedidoRepository.Add(order);
    }
    else
    {
      var existingOrderItem = order.OrderItemExists(orderItem);
      order.AddItem(orderItem);

      if (!existingOrderItem)
      {
        _pedidoRepository.AddItem(orderItem);
      }
      else
      {
        _pedidoRepository.UpdateItem(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
      }
      _pedidoRepository.Update(order);
    }

    order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.Name, message.Quantity, message.UnityValue));

    return await _pedidoRepository.UnitOfWork.Commit();
  }

  private bool ValidateCommand(AddOrderItemCommand message)
  {
    if (!message.IsValid())
    {
      foreach (var error in message.ValidationResult.Errors)
      {
        _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
      }
      return message.IsValid();
    }
    return message.IsValid();
  }
}
