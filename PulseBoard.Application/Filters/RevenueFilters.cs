using PulseBoard.Application.Common.Enums;

namespace PulseBoard.Application.Filters;

public sealed record RevenueFilters(
    Region? Region = null,
    Segment? Segment = null,
    string? ProductLine = null,
    int? CustomerId = null
);
