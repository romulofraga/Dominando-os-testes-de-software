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

        var orderItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var pedidoItem2 = new PedidoItem(productId, "Produto Teste", 1, 100);
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
        var orderItem = new PedidoItem(productId, "Produto Teste", Order.MAX_ITEM_UNITS, 100);
        order.AddItem(orderItem);

        var pedidoItem2 = new PedidoItem(productId, "Produto Teste", 1, 100);
        // Act & Assert
        Assert.Throws<DomainException>(() => order.AddItem(pedidoItem2));
    }

    [Fact(DisplayName = "Update inexistent Order Item")]
    [Trait("Category", "Sales - Order")]
    public void UpdateOrderItem_InexistentItem_ShouldThrowException()
    {
        // Arrange
        var order = Order.PedidoFactory.NewDraftOrder(Guid.NewGuid());
        var orderItem = new PedidoItem(Guid.NewGuid(), "Product Name", 5, 100);

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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var updatedOrderItem = new PedidoItem(productId, "Produto Teste", 5, 100);
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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var productId2 = Guid.NewGuid();
        var orderItem2 = new PedidoItem(productId2, "Produto Teste 2", 2, 50);
        order.AddItem(orderItem2);

        var updatedOrderItem = new PedidoItem(productId, "Produto Teste", 5, 100);
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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);
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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);

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
        var orderItem = new PedidoItem(productId, "Produto Teste", 2, 100);
        order.AddItem(orderItem);

        var productId2 = Guid.NewGuid();
        var orderItem2 = new PedidoItem(productId2, "Produto Teste 2", 2, 50);
        order.AddItem(orderItem2);

        // Act
        order.RemoveItem(orderItem);

        // Assert
        Assert.Equal(100, order.TotalValue);
    }
}
