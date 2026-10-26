
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quotes")]
    [ApiController]
    public class QuoteApprovalController : ControllerBase
    {
        IQuoteApprovalService service;

        public QuoteApprovalController(IQuoteApprovalService service)
        {
            this.service = service;
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveQuote(int id)
        {
            try
            {
                await service.ApproveQuote(id);

                return Ok(new { msg = "Quote approved" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectQuote(int id)
        {
            try
            {
                await service.RejectQuote(id);

                return Ok(new { msg = "Quote rejected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
