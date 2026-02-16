namespace PulseBoard.Application.DTOs.Revenue;

public record RevenueVarianceDto(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    decimal ActualRevenue,
    decimal TargetRevenue,
    decimal Delta,
    decimal PercentVariance
);
