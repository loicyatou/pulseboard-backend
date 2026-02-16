using PulseBoard.Application.Common.Enum;
using PulseBoard.Application.Common.Enums;
using PulseBoard.Application.DTOs.Revenue;
using PulseBoard.Application.Filters;

namespace PulseBoard.Application.Interfaces;

public interface IRevenueRepository
{
    Task<decimal> GetRevenueTotalAsync(DateTime start, DateTime end, RevenueFilters? filters = null);

    Task<IReadOnlyList<RevenueTrendPointDto>> GetRevenueTrendAsync(
        DateTime start,
        DateTime end,
        BucketType bucket,
        RevenueFilters? filters = null
    );

    Task<IReadOnlyList<RevenueBreakdownDto>> GetRevenueBreakdownAsync(
        DateTime start,
        DateTime end,
        RevenueGroupBy groupBy,
        RevenueFilters? filters = null
    );

    Task<RevenueComparisonDto> ComparePeriodsAsync(
        DateRange periodA,
        DateRange periodB,
        RevenueFilters? filters = null
    );

    Task<RevenueVarianceDto> CompareToTargetAsync(
        DateRange period,
        RevenueFilters? filters = null
    );
}
