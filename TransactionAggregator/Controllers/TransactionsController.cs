using Microsoft.AspNetCore.Mvc;
using TransactionAggregator.Attributes;
using TransactionAggregator.Models;
using TransactionAggregator.Services;

namespace TransactionAggregator.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [ApiKeyAuth]
  public class TransactionsController : ControllerBase
  {
    private readonly IAggregatorService _aggregatorService;
    public TransactionsController(IAggregatorService aggregatorService)
    {
        _aggregatorService = aggregatorService;
    }

    [HttpGet]
    public async Task<ActionResult<StandardizedResponse<IEnumerable<Transaction>>>> GetTransactions(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? category,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // sanitize pagination bounds
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 50) pageSize = 10;

        var (data, totalCount) = await _aggregatorService.GetTransactionsAsync(startDate, endDate, category, page, pageSize);

        var response = new StandardizedResponse<IEnumerable<Transaction>>
        {
            Data = data,
            Pagination = new PaginationMetaData
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalCount
            }
        };

        return Ok(response);
    }
  }
}