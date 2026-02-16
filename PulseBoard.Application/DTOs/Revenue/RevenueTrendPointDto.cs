namespace PulseBoard.Application.DTOs.Revenue;

public record RevenueTrendPointDto(
    DateTime PeriodStart,
    decimal Revenue
);
