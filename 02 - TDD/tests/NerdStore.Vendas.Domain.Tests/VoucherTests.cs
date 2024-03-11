using static NerdStore.Sales.Domain.Voucher;

namespace NerdStore.Sales.Domain.Tests;

public class VoucherTests
{
  [Fact(DisplayName = "Validate Voucher type value valid")]
  [Trait("Category", "Sales - Voucher")]
  public void Voucher_ValidateVoucherTypeValueValid_MustBeValid()
  {
    // Arrange
    var voucher = new Voucher("PROMO-15-REAIS", VoucherType.Value, 1, true, false, DateTime.Now.AddDays(2), null, 15);
    // Act
    var result = voucher.ValidateVoucher();
    // Assert
    Assert.True(result.IsValid);
  }

  [Fact(DisplayName = "Validate Voucher type value invalid")]
  [Trait("Category", "Sales - Voucher")]
  public void Voucher_ValidateVoucherTypeValueInvalid_MustBeInvalid()
  {
    // Arrange
    var voucher = new Voucher(string.Empty, VoucherType.Value, 0, false, true, DateTime.Now, null, null);
    var expectedErrors = new List<string>
    {
      VoucherValidation.CodeErrorMessage,
      VoucherValidation.ExpireDateErrorMessage,
      VoucherValidation.QuantityErrorMessage,
      VoucherValidation.ActiveErrorMessage,
      VoucherValidation.UsedErrorMessage,
      VoucherValidation.DiscountValueErrorMessage
    };
    // Act
    var result = voucher.ValidateVoucher();
    // Assert
    Assert.False(result.IsValid);
    Assert.True(result.Errors.Count == expectedErrors.Count);
    Assert.True(result.Errors.Select(error => error.ErrorMessage).SequenceEqual(expectedErrors));
  }
}