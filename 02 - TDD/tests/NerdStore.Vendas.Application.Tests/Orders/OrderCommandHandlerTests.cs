using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Domain;
using NerdStore.Vendas.Application.Orders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Tests.Orders;

public class OrderCommandHandlerTests
{
  //Add new order item with sucess
  [Fact(DisplayName = "Add new order item with sucess")]
  [Trait("Category", "Sales - Order Command Handler")]
  public async Task AddOrderItem_NewOrderItem_ShouldAddWithSuccess()
  {
    // Arrange
    var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Product Name", 2, 100);

    var mocker = new AutoMocker();
    var pedidoHandler = mocker.CreateInstance<OrderCommandHandler>();

    // Act
    var result = await pedidoHandler.Handle(orderCommand, CancellationToken.None);

    // Assert
    Assert.True(result);
    mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
    mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
  }
}