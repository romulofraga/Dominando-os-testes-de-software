using NerdStore.Core;
using NerdStore.Sales.Domain;

namespace NerdStore.Vendas.Domain;

public interface IOrderRepository : IRepository<Order>
{
  void AddItem(OrderItem orderItem);
  Task<Order> GetDraftOrderByClientId(Guid clientId);
  void UpdateItem(OrderItem orderItem);
}
