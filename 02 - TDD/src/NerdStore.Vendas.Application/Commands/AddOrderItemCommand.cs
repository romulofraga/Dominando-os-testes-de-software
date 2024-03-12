
using FluentValidation;
using FluentValidation.Results;

namespace NerdStore.Vendas.Application;

public class AddOrderItemCommand
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

  public ValidationResult IsValid()
  {
    return new AddOrderItemCommandValidation().Validate(this);
  }

  public class AddOrderItemCommandValidation : AbstractValidator<AddOrderItemCommand>
  {
    public AddOrderItemCommandValidation()
    {
      RuleFor(c => c.CustomerId)
          .NotEqual(Guid.Empty)
          .WithMessage("Invalid customer id");

      RuleFor(c => c.ProductId)
          .NotEqual(Guid.Empty)
          .WithMessage("Invalid product id");

      RuleFor(c => c.Name)
          .NotEmpty()
          .WithMessage("The product name was not provided");

      RuleFor(c => c.Quantity)
          .GreaterThan(0)
          .WithMessage("The minimum quantity of an item is 1");

      RuleFor(c => c.Quantity)
          .LessThanOrEqualTo(15)
          .WithMessage("The maximum quantity of an item is 15");

      RuleFor(c => c.UnityValue)
          .GreaterThan(0)
          .WithMessage("The unit value must be greater than 0");
    }
  }
}
