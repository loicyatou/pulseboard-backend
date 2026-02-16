using PulseBoard.Services.Enum;

namespace PulseBoard.Services.Entities;

public class Customer
{
   public int ID {get;set;}
   public required string Name {get;set;}
   public Segment Segmenet {get;set;}
   public Region Regioun {get;set;} 
   public int ChurnRiskScore {get => ChurnRiskScore;set => ChurnRiskScore = value >= 0 &&  value < 100 ? value : throw new ArgumentException("Risk score is out of bound");}
}
