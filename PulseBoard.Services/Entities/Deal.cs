using PulseBoard.Application.Common.Enums;

namespace PulseBoard.Services.Entities;

public class Deal
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GBP";

    public Stage Stage { get; set; }

    public DateTime ExpectedCloseDate { get; set; }

    private decimal _winProbability;

    /// <summary>
    /// Probability between 0 and 1 (e.g. 0.75m).
    /// </summary>
    public decimal WinProbability
    {
        get => _winProbability;
        set => _winProbability = value is >= 0m and <= 1m
            ? value
            : throw new ArgumentException("WinProbability must be between 0 and 1.");
    }

    private int _riskScore;

    public int RiskScore
    {
        get => _riskScore;
        set => _riskScore = value is >= 0 and <= 100
            ? value
            : throw new ArgumentException("RiskScore must be between 0 and 100.");
    }

    public string? Owner { get; set; }
}
