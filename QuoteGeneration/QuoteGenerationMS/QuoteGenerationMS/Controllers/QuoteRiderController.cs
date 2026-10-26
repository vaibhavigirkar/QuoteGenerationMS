
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quote-riders")]
    [ApiController]
    public class QuoteRiderController : ControllerBase
    {
        IQuoteRiderService service;

        public QuoteRiderController(IQuoteRiderService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddRider(createQuoteRiderDTO dto)
        {
            try
            {
                await service.AddRider(dto);
                return Ok(new { msg = "Rider added to quote" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{quote_id}")]
        public async Task<IActionResult> FetchRidersByQuoteId(int quote_id)
        {
            var data = await service.FetchRidersByQuoteId(quote_id);

            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRider(int id)
        {
            try
            {
                await service.RemoveRider(id);
                return Ok(new { msg = "Rider removed from quote" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
