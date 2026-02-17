using PulseBoard.Application.Common.Enums;

namespace PulseBoard.Services.Entities;

public class Order
{
   public int Id { get; set; }
   public int CustomerId { get; set; }

   public decimal Amount { get; set; }
   public string Currency { get; set; } = "GBP";

   public DateTime OrderDate { get; set; }
   public required string ProductLine { get; set; }
   public Customer Customer { get; set; } = null!;
   public Region Region { get; set; }
   public bool IsRecurring { get; set; }
}
