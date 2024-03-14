using static NerdStore.Vendas.Application.AddOrderItemCommand;

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
    Assert.True(result);
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
    Assert.False(result);
    Assert.Contains(AddOrderItemCommandValidation.InvalidCustomerIdMessage, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    Assert.Contains(AddOrderItemCommandValidation.InvalidProductIdMessage, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    Assert.Contains(AddOrderItemCommandValidation.EmptyProductNameMessage, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
    Assert.Contains(AddOrderItemCommandValidation.MinimumQuantityMessage, command.ValidationResult.Errors.Select(e => e.ErrorMessage));
  }
}