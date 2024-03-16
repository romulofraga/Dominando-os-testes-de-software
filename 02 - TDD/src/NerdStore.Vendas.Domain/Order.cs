using FluentValidation.Results;
using NerdStore.Core;

namespace NerdStore.Sales.Domain;

public class Order : Entity, IAggregateRoot
{
    public Guid CustomerId { get; private set; }
    public decimal TotalValue { get; private set; }
    public decimal DiscountValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public bool VoucherUsed { get; private set; }
    public Voucher Voucher { get; private set; }

    private readonly List<OrderItem> _orderItems;
    public static readonly int MAX_ITEM_UNITS = 15;
    public static readonly int MIN_ITEM_UNITS = 1;

    private Order() => _orderItems = [];

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public void SetDraft() => OrderStatus = OrderStatus.Draft;

    public bool OrderItemExists(OrderItem orderItem) => _orderItems.Any(item => item.ProductId == orderItem.ProductId);

    private void ValidateOrderItemQuantity(OrderItem orderItem)
    {
        var itemQuantity = orderItem.Quantity;
        if (OrderItemExists(orderItem))
        {
            var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            itemQuantity += existingItem.Quantity;
        }
        if (itemQuantity > MAX_ITEM_UNITS)
            throw new DomainException($"Maximum of {MAX_ITEM_UNITS} units per product exceeded");
    }

    public void AddItem(OrderItem orderItem)
    {
        ValidateOrderItemQuantity(orderItem);

        if (OrderItemExists(orderItem))
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
        CalculateOrderDiscount();
    }

    private bool IsValid()
    {
        throw new NotImplementedException();
    }

    public void UpdateItem(OrderItem orderItem)
    {
        ValidateOrderItemExistence(orderItem);
        ValidateOrderItemQuantity(orderItem);
        var orderItemExistence = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

        _orderItems.Remove(orderItemExistence);
        _orderItems.Add(orderItem);

        CalculateOrderValue();
    }

    private void ValidateOrderItemExistence(OrderItem orderItem)
    {
        if (!OrderItemExists(orderItem))
            throw new DomainException("Item not found in order");
    }

    public void RemoveItem(OrderItem orderItem)
    {
        ValidateOrderItemExistence(orderItem);
        _orderItems.Remove(orderItem);
        CalculateOrderValue();
    }

    public ValidationResult ApplyVoucher(Voucher voucher)
    {
        var result = voucher.ValidateVoucher();
        if (!result.IsValid) return result;
        Voucher = voucher;
        VoucherUsed = true;

        CalculateOrderDiscount();

        return result;
    }

    public void CalculateOrderDiscount()
    {
        if (!VoucherUsed) return;
        decimal discount = 0;

        if (Voucher.VoucherType is VoucherType.Percentage)
        {
            if (Voucher.Percentage.HasValue)
                discount = TotalValue * (Voucher.Percentage.Value / 100);
        }
        if (Voucher.VoucherType is VoucherType.Value)
        {
            if (Voucher.DiscountValue.HasValue)
                discount = Voucher.DiscountValue.Value;
        }

        TotalValue -= discount;
        if (TotalValue < 0) TotalValue = 0;
        DiscountValue = discount;


    }

    public static class OrderFactory
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
