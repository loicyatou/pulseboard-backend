using Microsoft.AspNetCore.Mvc;
using PulseBoard.Application.Interfaces;

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

    [HttpGet("total")]
    public async Task<IActionResult> GetTotal(DateTime start, DateTime end)
    {
        var total = await _revenueRepository.GetRevenueTotalAsync(start, end);
        return Ok(total);
    }
}
