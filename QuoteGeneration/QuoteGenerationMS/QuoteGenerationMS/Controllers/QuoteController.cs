
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/quotes")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService service;

        public QuoteController(IQuoteService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote(createQuoteDTO dto)
        {
            try
            {
                await service.CreateQuote(dto);
                return Ok(new { msg = "Quote created" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuoteById(int id)
        {
            var data = await service.GetQuoteById(id);

            if (data == null)
                return NotFound(new { msg = "Quote not found" });

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuotes()
        {
            return Ok(await service.GetAllQuotes());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(
            int id, updateQuoteDTO dto)
        {
            try
            {
                await service.UpdateQuote(id, dto);
                return Ok(new { msg = "Quote updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelQuote(int id)
        {
            try
            {
                await service.CancelQuote(id);
                return Ok(new { msg = "Quote cancelled" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpPost("{quoteId}/send-for-underwriting")]
        public async Task<IActionResult> SendQuoteForUnderwriting(
            int quoteId, [FromQuery] int customerId)
        {
            try
            {
                var underwritingId =
                    await service.SendQuoteForUnderwriting(
                        quoteId, customerId);

                return Ok(new
                {
                    msg = "Quote sent for underwriting successfully.",
                    quote_id = quoteId,
                    underwriting_id = underwritingId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpPost("{quoteId}/risk-score")]
        public async Task<IActionResult> CalculateRiskScore(
            int quoteId, [FromBody] calculateRiskScoreRequestDTO dto)
        {
            try
            {
                await service.CalculateUnderwritingRiskScore(quoteId, dto);

                return Ok(new
                {
                    msg = "Underwriting risk score calculated successfully.",
                    quote_id = quoteId
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { msg = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{quoteId}/underwriting")]
        public async Task<IActionResult> GetUnderwritingCase(int quoteId)
        {
            try
            {
                var result = await service.GetUnderwritingCase(quoteId);

                if (result == null)
                    return NotFound(new { msg = "Underwriting case not found." });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { msg = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{quoteId}/risk-score")]
        public async Task<IActionResult> GetRiskScore(int quoteId)
        {
            try
            {
                var result =
                    await service.GetUnderwritingRiskScore(quoteId);

                if (result == null)
                    return NotFound(new { msg = "Risk score not found." });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { msg = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{quoteId}/underwriting-decision")]
        public async Task<IActionResult> GetUnderwritingDecision(int quoteId)
        {
            try
            {
                var result =
                    await service.GetUnderwritingDecision(quoteId);

                if (result == null)
                    return NotFound(new { msg = "Underwriting decision not found." });

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { msg = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
