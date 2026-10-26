
using Microsoft.AspNetCore.Mvc;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using Microsoft.AspNetCore.Authorization;

namespace QuoteGenerationMS.Controllers
{
    [Authorize]
    [Route("api/v1/discounts")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        IDiscountService service;

        public DiscountController(IDiscountService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscount(createDiscountDTO dto)
        {
            try
            {
                await service.AddDiscount(dto);
                return Ok(new { msg = "Discount added" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FindDiscountById(int id)
        {
            var data = await service.FindDiscountById(id);

            if (data == null)
                return NotFound(new { msg = "Discount not found" });

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllDiscounts()
        {
            var data = await service.FetchAllDiscounts();
            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(
            int id, createDiscountDTO dto)
        {
            try
            {
                await service.UpdateDiscount(id, dto);
                return Ok(new { msg = "Discount updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateDiscount(int id)
        {
            try
            {
                await service.DeactivateDiscount(id);
                return Ok(new { msg = "Discount deactivated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
    }
}
