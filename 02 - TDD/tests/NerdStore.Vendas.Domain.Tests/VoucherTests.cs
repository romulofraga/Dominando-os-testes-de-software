namespace NerdStore.Sales.Domain.Tests;

public class VoucherTests
{
  //test to validate the voucher
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
}