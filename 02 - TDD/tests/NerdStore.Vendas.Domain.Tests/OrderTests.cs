namespace NerdStore.Sales.Domain.Tests;

public class OrderTests
{
    //create a test with fact and trait
    [Fact(DisplayName = "New Order Item to Empty Order")]
    [Trait("Category", "OrderTests")]
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
    [Trait("Category", "OrderTests")]
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
}
