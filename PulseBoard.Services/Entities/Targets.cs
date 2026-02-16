using PulseBoard.Application.Common.Enums;

namespace PulseBoard.Services.Entities;

public class Target
{
    public int Id { get; set; }

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    public Region? Region { get; set; }
    public Segment? Segment { get; set; }

    public string? ProductLine { get; set; }

    public decimal TargetRevenue { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
