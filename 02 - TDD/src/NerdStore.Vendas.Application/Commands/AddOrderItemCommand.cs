
using FluentValidation;
using FluentValidation.Results;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application;

public class AddOrderItemCommand : Command
{
  public Guid CustomerId { get; set; }

  public AddOrderItemCommand(Guid customerId, Guid productId, string name, int quantity, decimal unityValue)
  {
    CustomerId = customerId;
    ProductId = productId;
    Quantity = quantity;
    UnityValue = unityValue;
    Name = name;
  }

  public Guid ProductId { get; set; }
  public string Name { get; set; }
  public int Quantity { get; set; }
  public decimal UnityValue { get; set; }



  public override bool IsValid()
  {
    ValidationResult = new AddOrderItemCommandValidation().Validate(this);
    return ValidationResult.IsValid;
  }

  public class AddOrderItemCommandValidation : AbstractValidator<AddOrderItemCommand>
  {
    public AddOrderItemCommandValidation()
    {
      RuleFor(c => c.CustomerId)
        .NotEqual(Guid.Empty)
        .WithMessage(InvalidCustomerIdMessage);

      RuleFor(c => c.ProductId)
        .NotEqual(Guid.Empty)
        .WithMessage(InvalidProductIdMessage);

      RuleFor(c => c.Name)
        .NotEmpty()
        .WithMessage(EmptyProductNameMessage);

      RuleFor(c => c.Quantity)
        .GreaterThan(0)
        .WithMessage(MinimumQuantityMessage);

      RuleFor(c => c.Quantity)
        .LessThanOrEqualTo(15)
        .WithMessage(MaximumQuantityMessage);

      RuleFor(c => c.UnityValue)
        .GreaterThan(0)
        .WithMessage(InvalidUnityValueMessage);
    }

    public static string InvalidCustomerIdMessage { get; } = "Invalid customer id";
    public static string InvalidProductIdMessage { get; } = "Invalid product id";
    public static string EmptyProductNameMessage { get; } = "The product name was not provided";
    public static string MinimumQuantityMessage { get; } = "The minimum quantity of an item is 1";
    public static string MaximumQuantityMessage { get; } = "The maximum quantity of an item is 15";
    public static string InvalidUnityValueMessage { get; } = "The unit value must be greater than 0";
  }
}
