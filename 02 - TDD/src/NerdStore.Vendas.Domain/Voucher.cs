namespace NerdStore.Sales.Domain;

public class Voucher
{
  public string Code { get; private set; }
  public decimal? Percentage { get; private set; }
  public decimal? DiscountValue { get; private set; }
  public int Quantity { get; private set; }
  public DateTime CreatedDate { get; private set; }
  public DateTime? UsedDate { get; private set; }
  public DateTime ExpireDate { get; private set; }
  public bool Active { get; private set; }
}