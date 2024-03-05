using NerdStore.Core;
namespace NerdStore.Sales.Domain;

public class PedidoItem
{
  public Guid ProductId { get; private set; }
  public string ProductName { get; private set; }
  public int Quantity { get; private set; }
  private decimal UnitPrice { get; set; }

  public PedidoItem(Guid productId, string productName, int quantity, decimal unitPrice)
  {
    if (quantity > Order.MAX_ITEM_UNITS)
      throw new DomainException($"Maximum of {Order.MAX_ITEM_UNITS} units per product exceeded");
    if (quantity < Order.MIN_ITEM_UNITS)
      throw new DomainException($"Minimum of {Order.MIN_ITEM_UNITS} unit per product");

    ProductId = productId;
    ProductName = productName;
    Quantity = quantity;
    UnitPrice = unitPrice;
  }

  internal void AddUnits(int units)
  {
    Quantity += units;
  }

  internal decimal CalculateValue() => Quantity * UnitPrice;
}