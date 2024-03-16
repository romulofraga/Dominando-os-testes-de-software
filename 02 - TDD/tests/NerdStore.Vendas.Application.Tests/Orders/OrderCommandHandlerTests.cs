using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Domain;
using NerdStore.Vendas.Application.Orders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Tests.Orders;

public class OrderCommandHandlerTests
{
  //Add new order item with success
  [Fact(DisplayName = "Add new order item with success")]
  [Trait("Category", "Sales - Order Command Handler")]
  public async Task AddOrderItem_NewOrderItem_ShouldAddWithSuccess()
  {
    // Arrange
    var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Product Name", 2, 100);

    var mocker = new AutoMocker();
    var pedidoHandler = mocker.CreateInstance<OrderCommandHandler>();

    mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

    // Act
    var result = await pedidoHandler.Handle(orderCommand, CancellationToken.None);

    // Assert
    Assert.True(result);
    mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
    mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    //mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
  }

  [Fact(DisplayName = "Add order item with existing order draft")]
  [Trait("Category", "Sales - Order Command Handler")]
  public async Task AddOrderItem_ExistingOrderItem_ShouldAddWithSuccess()
  {
    // Arrange
    var clientId = Guid.NewGuid();
    var productId = Guid.NewGuid();

    var order = Order.OrderFactory.NewDraftOrder(clientId);
    var orderItem = new OrderItem(productId, "Product Name", 2, 100);
    order.AddItem(orderItem);

    var orderCommand = new AddOrderItemCommand(clientId, productId, "Product Name", 2, 100);

    var mocker = new AutoMocker();
    var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

    mocker.GetMock<IOrderRepository>().Setup(r => r.GetDraftOrderByClientId(clientId)).Returns(Task.FromResult(order));
    mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

    // Act
    var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

    // Assert
    Assert.True(result);
    mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
    mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
  }

    // add order item invalid command should return false and launch event
    [Fact(DisplayName = "Add order item invalid command should return false and launch event")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddOrderItem_InvalidCommand_ShouldReturnFalseAndLaunchEvent()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.Empty, "", 0, 0);

        var mocker = new AutoMocker();
        var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

        // Act
        var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.False(result);
        mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(4));
    }

}