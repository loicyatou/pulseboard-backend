using PulseBoard.Application.Common.Enums;

namespace PulseBoard.Services.Entities;

public class Customer
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public Segment Segment { get; set; }

    public Region Region { get; set; }

    private int _churnRiskScore;

    public int ChurnRiskScore
    {
        get => _churnRiskScore;
        set => _churnRiskScore = value is >= 0 and <= 100
            ? value
            : throw new ArgumentException("Churn risk score must be between 0 and 100.");
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Deal> Deals { get; set; } = new List<Deal>();
}
