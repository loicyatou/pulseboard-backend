using PulseBoard.Services.Enum;

namespace PulseBoard.Services.Entities;

public class Order
{
   public int ID {get;set;}
   public int CustomerID {get;set;}

   public Decimal Amount {get;set;}
   public string Currency {get;set;} = "Sterling";

   public DateTime OrderDate {get;set;}
   public string ProductLine {get;set;}
   public Region Regioun {get;set;} 
   public bool IsRecurring {get;set;}
}
