using System.Text.Json.Serialization;
using PulseBoard.Application.Common.Enum;
using PulseBoard.Application.Common.Enums;
using PulseBoard.Application.Filters;

namespace PulseBoard.Presentation.Contracts.Revenue;

public sealed record RevenueTotalRequest(
    DateTime Start,
    DateTime End,
    RevenueFilters? Filters);

public sealed record RevenueBreakdownRequest(
    DateTime Start,
    DateTime End,
    RevenueGroupBy GroupBy,
    RevenueFilters? Filters);

public sealed record RevenueTrendRequest(
    [property: JsonPropertyName("start")] DateTime Start,
    [property: JsonPropertyName("end")] DateTime End,
    [property: JsonPropertyName("bucket")] BucketType Bucket,
    [property: JsonPropertyName("filters")] RevenueFilters? Filters);

public sealed record RevenueComparePeriodsRequest(
    DateTime PeriodAStart,
    DateTime PeriodAEnd,
    DateTime PeriodBStart,
    DateTime PeriodBEnd,
    RevenueFilters? Filters);

public sealed record RevenueCompareToTargetRequest(
    DateTime PeriodStart,
    DateTime PeriodEnd,
    RevenueFilters? Filters);
