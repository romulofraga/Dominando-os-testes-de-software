using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Domain;
using NerdStore.Vendas.Application.Orders;
using NerdStore.Vendas.Domain;

namespace NerdStore.Vendas.Application.Tests.Orders;

public class OrderCommandHandlerTests
{
    private readonly Guid _clientId;
    private readonly Guid _productId;
    private readonly Order _order;
    private readonly AutoMocker _mocker;
    private readonly OrderCommandHandler _orderCommandHandler;
    public OrderCommandHandlerTests()
    {
        _clientId = Guid.NewGuid();
        _productId = Guid.NewGuid();
        _order = Order.OrderFactory.NewDraftOrder(_clientId);
        _mocker = new AutoMocker();
        _orderCommandHandler = _mocker.CreateInstance<OrderCommandHandler>();
    }

  //Add new order item with success
  [Fact(DisplayName = "Add new order item with success")]
  [Trait("Category", "Sales - Order Command Handler")]
  public async Task AddOrderItem_NewOrderItem_ShouldAddWithSuccess()
  {
    // Arrange
    var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Product Name", 2, 100);

    _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

    // Act
    var result = await _orderCommandHandler.Handle(orderCommand, CancellationToken.None);

    // Assert
    Assert.True(result);
    _mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
    _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
    //mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
  }

  [Fact(DisplayName = "Add order item with existing order draft")]
  [Trait("Category", "Sales - Order Command Handler")]
  public async Task AddOrderItem_ExistingOrderItem_ShouldAddWithSuccess()
  {
    // Arrange

    var orderItem = new OrderItem(_productId, "Product Name", 2, 100);
    _order.AddItem(orderItem);

    var orderCommand = new AddOrderItemCommand(_clientId, _productId, "Product Name", 2, 100);

    _mocker.GetMock<IOrderRepository>().Setup(r => r.GetDraftOrderByClientId(_clientId)).Returns(Task.FromResult(_order));
    _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

    // Act
    var result = await _orderCommandHandler.Handle(orderCommand, CancellationToken.None);

    // Assert
    Assert.True(result);
    _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
    _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
    _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
  }

    // add order item invalid command should return false and launch event
    [Fact(DisplayName = "Add order item invalid command should return false and launch event")]
    [Trait("Category", "Sales - Order Command Handler")]
    public async Task AddOrderItem_InvalidCommand_ShouldReturnFalseAndLaunchEvent()
    {
        // Arrange
        var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.Empty, "", 0, 0);

        // Act
        var result = await _orderCommandHandler.Handle(orderCommand, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(4));
    }

}