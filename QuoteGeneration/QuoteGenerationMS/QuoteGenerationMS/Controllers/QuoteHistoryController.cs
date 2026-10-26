
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quote-history")]
    [ApiController]
    public class QuoteHistoryController : ControllerBase
    {
        IQuoteHistoryService service;

        public QuoteHistoryController(IQuoteHistoryService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllHistory()
        {
            var data = await service.FetchAllHistory();

            return Ok(data);
        }

        [HttpGet("{quote_id}")]
        public async Task<IActionResult> FetchHistoryByQuoteId(
            int quote_id)
        {
            try
            {
                var data = await service.FetchHistoryByQuoteId(quote_id);

                return Ok(data);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Quote not found.")
                    return NotFound(new { msg = ex.Message });

                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
