using NerdStore.Core;

namespace NerdStore.Sales.Domain.Tests;

public class OrderTests
{
    [Fact(DisplayName = "New Order Item to Empty Order")]
    [Trait("Category", "Sales - Order")]
    public void AddOrderItem_NewOrder_ShouldUpdateValue()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());

        var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 2, 100);

        // Act
        order.AddItem(orderItem);

        // Assert
        Assert.Equal(200, order.TotalValue);
    }

    [Fact(DisplayName = "Add new Order Item to Order")]
    [Trait("Category", "Sales - Order")]
    public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadeSomarValores()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var pedidoItem2 = new OrderItem(productId, "Produto Teste", 1, 100);
        // Act
        order.AddItem(pedidoItem2);
        // Assert
        Assert.Equal(300, order.TotalValue);
        Assert.Equal(1, order.OrderItems.Count);
        Assert.Equal(3, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
    }

    [Fact(DisplayName = "Add new Order Item with units above maximum")]
    [Trait("Category", "Sales - Order")]
    public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", Order.MAX_ITEM_UNITS, 100);
        order.AddItem(orderItem);

        var pedidoItem2 = new OrderItem(productId, "Produto Teste", 1, 100);
        // Act & Assert
        Assert.Throws<DomainException>(() => order.AddItem(pedidoItem2));
    }

    [Fact(DisplayName = "Update inexistent Order Item")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_InexistentItem_ShouldThrowException()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Product Name", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.UpdateItem(orderItem));
    }

    [Fact(DisplayName = "Update Order Item")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_ValidItem_ShouldUpdateQuantity()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var updatedOrderItem = new OrderItem(productId, "Produto Teste", 5, 100);
        // Act
        order.UpdateItem(updatedOrderItem);
        // Assert
        Assert.Equal(500, order.TotalValue);
        Assert.Equal(1, order.OrderItems.Count);
        Assert.Equal(5, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
    }

    [Fact(DisplayName = "Update Order Item with different items")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_DifferentItem_ShouldUpdateTotalValue()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var productId2 = Guid.NewGuid();
        var orderItem2 = new OrderItem(productId2, "Produto Teste 2", 2, 50);
        order.AddItem(orderItem2);

        var updatedOrderItem = new OrderItem(productId, "Produto Teste", 5, 100);
        // Act
        order.UpdateItem(updatedOrderItem);
        // Assert
        Assert.Equal(600, order.TotalValue);
        Assert.Equal(2, order.OrderItems.Count);
        Assert.Equal(5, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
    }
    [Fact(DisplayName = "Remove Order Item")]
    [Trait("Category", "Sales - Order")]
    public void RemoveItem_OrderItemExists_ShouldRemoveItem()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        // Act
        order.RemoveItem(orderItem);

        // Assert
        Assert.Equal(0, order.OrderItems.Count);
    }

    [Fact(DisplayName = "Remove Order Item that does not exist")]
    [Trait("Category", "Sales - Order")]
    public void RemoveItem_OrderItemDoesNotExist_ShouldThrowException()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.RemoveItem(orderItem));
    }

    [Fact(DisplayName = "Remove Order Item and update total value")]
    [Trait("Category", "Sales - Order")]
    public void RemoveItem_OrderItemExists_ShouldUpdateTotalValue()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var productId2 = Guid.NewGuid();
        var orderItem2 = new OrderItem(productId2, "Produto Teste 2", 2, 50);
        order.AddItem(orderItem2);

        // Act
        order.RemoveItem(orderItem);

        // Assert
        Assert.Equal(100, order.TotalValue);
    }

    [Fact(DisplayName = "Apply Voucher Valid")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_ValidVoucher_ShouldApplyVoucher()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var voucher = new Voucher("XPTO", VoucherType.Value, 10, true, false, DateTime.Now.AddDays(10), null, 50);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = "Apply Voucher Invalid")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_InvalidVoucher_ShouldReturnError()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var voucher = new Voucher(string.Empty, VoucherType.Value, 0, false, true, DateTime.Now, null, null);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = "Apply Voucher percentage")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_VoucherPercentage_ShouldDiscountTotalValue()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto Teste 2", 1, 200);

        order.AddItem(orderItem);
        order.AddItem(orderItem2);
        var voucher = new Voucher("XPTO", VoucherType.Percentage, 10, true, false, DateTime.Now.AddDays(10), 50, null);

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(200, order.TotalValue);
    }

    [Fact(DisplayName = "Apply Voucher value")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_VoucherValue_ShouldDiscountTotalValue()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto Teste 2", 1, 200);

        order.AddItem(orderItem);
        order.AddItem(orderItem2);
        var voucher = new Voucher("XPTO", VoucherType.Value, 10, true, false, DateTime.Now.AddDays(10), null, 15);

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(385, order.TotalValue);
    }

    // apply voucher value exceeds total value
    [Fact(DisplayName = "Apply Voucher value exceeds total value")]
    [Trait("Category", "Sales - Order")]
    public void ApplyVoucher_VoucherValueExceedsTotalValue_ShouldReturnZero()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto Teste 2", 1, 200);

        order.AddItem(orderItem);
        order.AddItem(orderItem2);
        var voucher = new Voucher("XPTO", VoucherType.Value, 10, true, false, DateTime.Now.AddDays(10), null, 600);

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(0, order.TotalValue);
    }

    // recalculate order discount after adding a voucher and a new order item
    [Fact(DisplayName = "Recalculate order discount after adding a voucher and a new order item")]
    [Trait("Category", "Sales - Order")]
    public void RecalculateOrderDiscount_VoucherAndNewItem_ShouldCalculateDiscount()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 1, 100);
        order.AddItem(orderItem);
        var voucher = new Voucher("XPTO", VoucherType.Percentage, 50, true, false, DateTime.Now.AddDays(10), 50, 0);
        order.ApplyVoucher(voucher);

        var orderItem2 = new OrderItem(Guid.NewGuid(), "Produto Teste 2", 1, 100);
        // Act
        order.AddItem(orderItem2);

        // Assert
        Assert.Equal(100, order.TotalValue);
    }
}
