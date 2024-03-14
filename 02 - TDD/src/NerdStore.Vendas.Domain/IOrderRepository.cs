using NerdStore.Sales.Domain;

namespace NerdStore.Vendas.Domain;

public interface IOrderRepository
{
  void Add(Order order);
}
