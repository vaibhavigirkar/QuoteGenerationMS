
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quotes")]
    [ApiController]
    public class PremiumCalculationController : ControllerBase
    {
        IPremiumCalculationService service;

        public PremiumCalculationController(
            IPremiumCalculationService service)
        {
            this.service = service;
        }

        [HttpPost("calculate-premium")]
        public async Task<IActionResult> CalculatePremium(
            calculatePremiumDTO dto)
        {
            try
            {
                await service.CalculatePremium(dto);

                return Ok(new { msg = "Premium calculated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{id}/premium")]
        public async Task<IActionResult> GetPremiumByQuoteId(int id)
        {
            var data = await service.GetPremiumByQuoteId(id);

            if (data == null)
                return NotFound(new { msg = "Premium calculation not found" });

            return Ok(data);
        }
    }
}
