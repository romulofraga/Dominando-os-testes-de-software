namespace NerdStore.Vendas.Application.Tests.Orders;

public class AddOrderItemCommandTests
{
  [Fact(DisplayName = "Add item command valid")]
  [Trait("Category", "Vendas - Order Commands")]
  public void AddOrderItemCommand_CommandValid_ShouldBeValid()
  {
    // Arrange
    var command = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

    // Act
    var result = command.IsValid();

    // Assert
    Assert.True(result.IsValid);
  }

  [Fact(DisplayName = "Add item command invalid")]
  [Trait("Category", "Vendas - Order Commands")]
  public void AddOrderItemCommand_CommandInvalid_ShouldBeInvalid()
  {
    // Arrange
    var command = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

    // Act
    var result = command.IsValid();

    // Assert
    Assert.False(result.IsValid);
  }

}