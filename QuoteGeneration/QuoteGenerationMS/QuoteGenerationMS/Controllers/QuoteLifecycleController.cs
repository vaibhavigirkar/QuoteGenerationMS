
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quotes")]
    [ApiController]
    public class QuoteLifecycleController : ControllerBase
    {
        IQuoteLifecycleService service;

        public QuoteLifecycleController(IQuoteLifecycleService service)
        {
            this.service = service;
        }

        [HttpPut("expire")]
        public async Task<IActionResult> ExpireQuotes()
        {
            try
            {
                await service.ExpireQuotes();

                return Ok(new { msg = "Expired quotes updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpPut("{id}/convert")]
        public async Task<IActionResult> ConvertQuote(int id)
        {
            try
            {
                await service.ConvertQuote(id);

                return Ok(new { msg = "Quote converted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
