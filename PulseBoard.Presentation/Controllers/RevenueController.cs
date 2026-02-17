using Microsoft.AspNetCore.Mvc;
using PulseBoard.Application.Interfaces;
using PulseBoard.Application.DTOs.Revenue;
using PulseBoard.Presentation.Contracts.Revenue;

namespace PulseBoard.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueRepository _revenueRepository;

    public RevenueController(IRevenueRepository revenueRepository)
    {
        _revenueRepository = revenueRepository;
    }

    [HttpPost("total")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotal([FromBody] RevenueTotalRequest request)
    {
        var total = await _revenueRepository.GetRevenueTotalAsync(
            request.Start,
            request.End,
            request.Filters);

        return Ok(total);
    }

    [HttpPost("breakdown")]
    [ProducesResponseType(typeof(IReadOnlyList<RevenueBreakdownDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBreakdown([FromBody] RevenueBreakdownRequest request)
    {
        var result = await _revenueRepository.GetRevenueBreakdownAsync(
            request.Start,
            request.End,
            request.GroupBy,
            request.Filters);

        return Ok(result);
    }

    [HttpPost("trend")]
    [ProducesResponseType(typeof(IReadOnlyList<RevenueTrendPointDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrend([FromBody] RevenueTrendRequest request)
    {
        var result = await _revenueRepository.GetRevenueTrendAsync(
            request.Start,
            request.End,
            request.Bucket,
            request.Filters);

        return Ok(result);
    }

    [HttpPost("compare-periods")]
    [ProducesResponseType(typeof(RevenueComparisonDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ComparePeriods([FromBody] RevenueComparePeriodsRequest request)
    {
        var result = await _revenueRepository.ComparePeriodsAsync(
            new DateRange(request.PeriodAStart, request.PeriodAEnd),
            new DateRange(request.PeriodBStart, request.PeriodBEnd),
            request.Filters);

        return Ok(result);
    }

    [HttpPost("compare-to-target")]
    [ProducesResponseType(typeof(RevenueVarianceDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompareToTarget([FromBody] RevenueCompareToTargetRequest request)
    {
        var result = await _revenueRepository.CompareToTargetAsync(
            new DateRange(request.PeriodStart, request.PeriodEnd),
            request.Filters);

        return Ok(result);
    }
}
