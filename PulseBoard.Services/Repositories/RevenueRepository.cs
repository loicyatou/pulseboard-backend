using Microsoft.EntityFrameworkCore;
using PulseBoard.Application.Common.Enum;
using PulseBoard.Application.Common.Enums;
using PulseBoard.Application.DTOs.Revenue;
using PulseBoard.Application.Filters;
using PulseBoard.Application.Interfaces;
using PulseBoard.Services.Data;
using PulseBoard.Services.Entities;

public class RevenueRepository : IRevenueRepository
{
    private readonly AppDbContext _context;
    public RevenueRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<RevenueComparisonDto> ComparePeriodsAsync(
        DateRange periodA,
        DateRange periodB,
        RevenueFilters? filters = null)
    {
        var queryA = BuildRevenueQuery(periodA.Start, periodA.End, filters);
        var queryB = BuildRevenueQuery(periodB.Start, periodB.End, filters);

        var revenueA = await queryA
            .Select(o => (decimal?)o.Amount)
            .SumAsync() ?? 0m;

        var revenueB = await queryB
            .Select(o => (decimal?)o.Amount)
            .SumAsync() ?? 0m;

        var delta = revenueA - revenueB;

        decimal percentChange = 0m;

        if (revenueB != 0m)
        {
            percentChange = delta / revenueB * 100m;
        }
        else if (revenueA != 0m)
        {
            // If previous period was 0 and current isn't,
            // treat as 100% growth 
            percentChange = 100m;
        }

        return new RevenueComparisonDto(
            PeriodAStart: periodA.Start,
            PeriodAEnd: periodA.End,
            PeriodARevenue: revenueA,
            PeriodBStart: periodB.Start,
            PeriodBEnd: periodB.End,
            PeriodBRevenue: revenueB,
            Delta: delta,
            PercentChange: percentChange
        );
    }

    public async Task<RevenueVarianceDto> CompareToTargetAsync(
        DateRange period,
        RevenueFilters? filters = null)
    {
        var revenueQuery = BuildRevenueQuery(period.Start, period.End, filters);

        var actualRevenue = await revenueQuery
            .Select(o => (decimal?)o.Amount)
            .SumAsync() ?? 0m;

        var targetQuery = _context.Targets
            .Where(t => t.PeriodStart == period.Start && t.PeriodEnd == period.End);

        if (filters is not null)
        {
            if (filters.Region is not null)
                targetQuery = targetQuery.Where(t => t.Region == filters.Region);

            if (filters.Segment is not null)
                targetQuery = targetQuery.Where(t => t.Segment == filters.Segment);

            if (!string.IsNullOrWhiteSpace(filters.ProductLine))
            {
                var productLine = filters.ProductLine.Trim().ToLower();
                targetQuery = targetQuery.Where(t => t.ProductLine.ToLower() == productLine);
            }
        }

        var targetRevenue = await targetQuery
            .Select(t => (decimal?)t.TargetRevenue)
            .SumAsync() ?? 0m;

        var delta = actualRevenue - targetRevenue;

        decimal percentVariance = 0m;

        if (targetRevenue != 0m)
            percentVariance = (delta / targetRevenue) * 100m;

        return new RevenueVarianceDto(
            PeriodStart: period.Start,
            PeriodEnd: period.End,
            ActualRevenue: actualRevenue,
            TargetRevenue: targetRevenue,
            Delta: delta,
            PercentVariance: percentVariance
        );
    }

    public async Task<IReadOnlyList<RevenueBreakdownDto>> GetRevenueBreakdownAsync(
        DateTime start,
        DateTime end,
        RevenueGroupBy groupBy,
        RevenueFilters? filters = null)
    {
        var query = BuildRevenueQuery(start, end, filters);

        switch (groupBy)
        {
            case RevenueGroupBy.Region:
                return await query
                    .GroupBy(o => o.Region.ToString())
                    .Select(g => new RevenueBreakdownDto(
                        g.Key,
                        g.Sum(x => x.Amount)))
                    .ToListAsync();

            case RevenueGroupBy.Segment:
                return await query
                    .GroupBy(o => o.Customer.Segment.ToString())
                    .Select(g => new RevenueBreakdownDto(
                      g.Key,
                     g.Sum(x => x.Amount)))
                    .ToListAsync();

            case RevenueGroupBy.ProductLine:
                return await query
                    .GroupBy(o => o.ProductLine)
                    .Select(g => new RevenueBreakdownDto(
                        g.Key,
                        g.Sum(x => x.Amount)))
                    .ToListAsync();

            case RevenueGroupBy.Customer:
                return await query
                    .GroupBy(o => o.Customer.Name)
                    .Select(g => new RevenueBreakdownDto(
                        g.Key,
                         g.Sum(x => x.Amount)))
                    .ToListAsync();

            default:
                throw new ArgumentOutOfRangeException(nameof(groupBy), groupBy, null);
        }
    }


    public async Task<decimal> GetRevenueTotalAsync(
        DateTime start,
        DateTime end,
        RevenueFilters? filters = null)
    {
        var query = BuildRevenueQuery(start, end, filters);

        return await query
            .Select(o => (decimal?)o.Amount)
            .SumAsync() ?? 0m;
    }


    public async Task<IReadOnlyList<RevenueTrendPointDto>> GetRevenueTrendAsync(
        DateTime start,
        DateTime end,
        BucketType bucket,
        RevenueFilters? filters = null)
    {
        var query = BuildRevenueQuery(start, end, filters);

        switch (bucket)
        {
            case BucketType.Daily:
                return await query
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new RevenueTrendPointDto(
                        g.Key,
                        g.Sum(x => x.Amount)))
                    .OrderBy(x => x.PeriodStart)
                    .ToListAsync();

            case BucketType.Monthly:
                return await query
                    .GroupBy(o => new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
                    .Select(g => new RevenueTrendPointDto(
                        g.Key,
                        g.Sum(x => x.Amount)))
                    .OrderBy(x => x.PeriodStart)
                    .ToListAsync();

            default:
                throw new ArgumentOutOfRangeException(nameof(bucket), bucket, null);
        }
    }


    private IQueryable<Order> BuildRevenueQuery(
    DateTime start,
    DateTime end,
    RevenueFilters? filters)
    {
        var query = _context.Orders
            .Where(o => o.OrderDate >= start && o.OrderDate <= end);

        if (filters is null)
            return query;

        if (filters.Region is not null)
            query = query.Where(o => o.Region == filters.Region);

        if (!string.IsNullOrWhiteSpace(filters.ProductLine))
        {
            var productLine = filters.ProductLine.Trim().ToLower();
            query = query.Where(o => o.ProductLine.ToLower() == productLine);
        }

        if (filters.CustomerId is not null)
            query = query.Where(o => o.CustomerId == filters.CustomerId);

        if (filters.Segment is not null)
            query = query.Where(o => o.Customer.Segment == filters.Segment);

        return query;
    }

}