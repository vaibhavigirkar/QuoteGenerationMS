
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/risk-assessment")]
    [ApiController]
    public class RiskAssessmentController : ControllerBase
    {
        IRiskAssessmentService service;

        public RiskAssessmentController(IRiskAssessmentService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRiskAssessment(
            createRiskAssessmentDTO dto)
        {
            try
            {
                await service.CreateRiskAssessment(dto);

                return Ok(new { msg = "Risk assessment completed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{quoteId}")]
        public async Task<IActionResult> GetRiskAssessmentByQuoteId(
            int quoteId)
        {
            var data = await service.GetRiskAssessmentByQuoteId(quoteId);

            if (data == null)
                return NotFound(new { msg = "Risk assessment not found" });

            return Ok(data);
        }
    }
}
