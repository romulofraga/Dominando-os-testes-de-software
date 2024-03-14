
using MediatR;
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

  public Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
  {
    var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnityValue);
    var order = Order.PedidoFactory.NewDraftOrder(message.CustomerId);

    order.AddItem(orderItem);

    _pedidoRepository.Add(order);
    _mediator.Publish(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.Name, message.Quantity, message.UnityValue), cancellationToken);
    return Task.FromResult(true);
  }
}
