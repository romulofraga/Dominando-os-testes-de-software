using FluentValidation.Results;
using NerdStore.Core;

namespace NerdStore.Sales.Domain;

public class Order
{
    public Guid CustomerId { get; private set; }
    public decimal TotalValue { get; private set; }
    public decimal DiscountValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public bool VoucherUsed { get; private set; }
    public Voucher Voucher { get; private set; }

    private readonly List<PedidoItem> _orderItems;
    public static readonly int MAX_ITEM_UNITS = 15;
    public static readonly int MIN_ITEM_UNITS = 1;

    private Order() => _orderItems = [];

    public IReadOnlyCollection<PedidoItem> OrderItems => _orderItems;

    public void SetDraft() => OrderStatus = OrderStatus.Draft;

    private bool OderItemExists(PedidoItem orderItem) => _orderItems.Any(item => item.ProductId == orderItem.ProductId);

    private void ValidateOrderItemQuantity(PedidoItem orderItem)
    {
        var itemQuantity = orderItem.Quantity;
        if (OderItemExists(orderItem))
        {
            var existingItem = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            itemQuantity += existingItem.Quantity;
        }
        if (itemQuantity > MAX_ITEM_UNITS)
            throw new DomainException($"Maximum of {MAX_ITEM_UNITS} units per product exceeded");
    }

    public void AddItem(PedidoItem orderItem)
    {
        ValidateOrderItemQuantity(orderItem);

        if (OderItemExists(orderItem))
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

    public void UpdateItem(PedidoItem orderItem)
    {
        ValidateOrderItemExistence(orderItem);
        ValidateOrderItemQuantity(orderItem);
        var orderItemExistence = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

        _orderItems.Remove(orderItemExistence);
        _orderItems.Add(orderItem);

        CalculateOrderValue();
    }

    private void ValidateOrderItemExistence(PedidoItem orderItem)
    {
        if (!OderItemExists(orderItem))
            throw new DomainException("Item not found in order");
    }

    public void RemoveItem(PedidoItem orderItem)
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
