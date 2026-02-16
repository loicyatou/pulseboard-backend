namespace PulseBoard.Application.DTOs.Revenue;

public record RevenueComparisonDto(
    DateTime PeriodAStart,
    DateTime PeriodAEnd,
    decimal PeriodARevenue,
    DateTime PeriodBStart,
    DateTime PeriodBEnd,
    decimal PeriodBRevenue,
    decimal Delta,
    decimal PercentChange
);
