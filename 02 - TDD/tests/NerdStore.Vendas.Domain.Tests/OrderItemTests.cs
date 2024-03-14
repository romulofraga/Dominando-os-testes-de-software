using NerdStore.Core;

namespace NerdStore.Sales.Domain.Tests;

public class OrderItemTests
{
    [Fact(DisplayName = "New Order Item with units below minimum")]
    [Trait("Category", "Sales - OrderItem")]
    public void OrderItem_NewOrderItem_UnitsBelowMinimum_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Product Name", Order.MIN_ITEM_UNITS - 1, 100));
    }

    [Fact(DisplayName = "New Order Item with units above maximum")]
    [Trait("Category", "Sales - OrderItem")]
    public void OrderItem_NewOrderItem_UnitsAboveMaximum_ShouldThrowException()
    {
        // Arrange & Act & Assert
        Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Product Name", Order.MAX_ITEM_UNITS + 1, 100));
    }
}