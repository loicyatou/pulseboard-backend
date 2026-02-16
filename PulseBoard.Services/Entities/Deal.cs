using PulseBoard.Services.Enum;

namespace PulseBoard.Services.Entities;

public class Deal
{
   public int ID {get;set;}
   public int CustomerID {get;set;}

   public Decimal Amount {get;set;}
   public Stage Stage {get;set;}

   public DateTime ExpectedCloseDate {get;set;}
   public int WinProbability {get;set;}
   public int RiskScore {get;set;}
   public string? Owner {get;set;}
}
