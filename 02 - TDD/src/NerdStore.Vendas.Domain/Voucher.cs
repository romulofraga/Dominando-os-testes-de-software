using FluentValidation;
using FluentValidation.Results;

namespace NerdStore.Sales.Domain;

public class Voucher(
                      string code,
                      VoucherType voucherType,
                      int quantity,
                      bool active,
                      bool used,
                      DateTime? expireDate = null,
                      decimal? percentage = null,
                      decimal? discountValue = null)
{
  public string Code { get; private set; } = code;
  public VoucherType VoucherType { get; private set; } = voucherType;
  public decimal? Percentage { get; private set; } = percentage;
  public decimal? DiscountValue { get; private set; } = discountValue;
  public int Quantity { get; private set; } = quantity;
  public DateTime CreatedDate { get; private set; } = DateTime.Now;
  public DateTime? UsedDate { get; private set; }
  public DateTime? ExpireDate { get; private set; } = expireDate;
  public bool Active { get; private set; } = active;
  public bool Used { get; private set; } = used;

  public ValidationResult ValidateVoucher()
  {
    return new VoucherValidation().Validate(this);
  }

  public class VoucherValidation : AbstractValidator<Voucher>
  {
    public static string CodeErrorMessage => "The voucher code is invalid";
    public static string ExpireDateErrorMessage => "The voucher has expired";
    public static string QuantityErrorMessage => "The voucher is no longer available";
    public static string ActiveErrorMessage => "The voucher is no longer active";
    public static string UsedErrorMessage => "The voucher has already been used";
    public static string DiscountValueErrorMessage => "The value of the voucher is invalid";
    public static string PercentageErrorMessage => "The percentage of the voucher is invalid";


    public VoucherValidation()
    {
      RuleFor(c => c.Code)
          .NotEmpty()
          .WithMessage(CodeErrorMessage);

      RuleFor(c => c.ExpireDate)
          .Must(BeValidExpireDate)
          .WithMessage(ExpireDateErrorMessage);

      RuleFor(c => c.Quantity)
          .GreaterThan(0)
          .WithMessage(QuantityErrorMessage);

      RuleFor(c => c.Active)
          .Equal(true)
          .WithMessage(ActiveErrorMessage);

      RuleFor(c => c.Used)
          .Equal(false)
          .WithMessage(UsedErrorMessage);

      When(f => f.VoucherType == VoucherType.Value, () =>
      {
        RuleFor(c => c.DiscountValue)
            .NotNull()
            .WithMessage(DiscountValueErrorMessage)
            .GreaterThan(0)
            .WithMessage(DiscountValueErrorMessage);
      });

      When(f => f.VoucherType == VoucherType.Percentage, () =>
      {
        RuleFor(c => c.Percentage)
            .NotNull()
            .WithMessage(PercentageErrorMessage)
            .GreaterThan(0)
            .WithMessage(PercentageErrorMessage)
            .LessThanOrEqualTo(100)
            .WithMessage(PercentageErrorMessage);
      });
    }

    private bool BeValidExpireDate(DateTime? expireDate)
    {
      if (!expireDate.HasValue) return true;
      return expireDate.Value >= DateTime.Now;
    }
  }
}

public enum VoucherType
{
  Percentage = 0,
  Value = 1
}