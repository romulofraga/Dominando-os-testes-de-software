namespace NerdStore.Sales.Domain;

public class Order
{
    public Guid CustomerId { get; private set; }
    public decimal TotalValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    private readonly List<PedidoItem> _orderItems;
    public static readonly int MAX_ITEM_UNITS = 15;
    public static readonly int MIN_ITEM_UNITS = 1;

    private Order() => _orderItems = new List<PedidoItem>();

    public IReadOnlyCollection<PedidoItem> OrderItems => _orderItems;

    public void SetDraft() => OrderStatus = OrderStatus.Draft;

    public void AddItem(PedidoItem orderItem)
    {
        if (_orderItems.Any(p => p.ProductId == orderItem.ProductId))
        {
            var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            existingItem.AddUnits(orderItem.Quantity);
            orderItem = existingItem;

            _orderItems.Remove(existingItem);
        }

        _orderItems.Add(orderItem);

        CalculateOrderValue();
    }

    private void CalculateOrderValue()
    {
        TotalValue = OrderItems.Sum(i => i.CalculateValue());
    }

    public bool IsValid()
    {
        throw new NotImplementedException();
    }

    public static class PedidoFactory
    {
        public static Order NewDraftOrder(Guid customerId)
        {
            var order = new Order
            {
                CustomerId = customerId
            };

            order.SetDraft();
            return order;
        }
    }
}
