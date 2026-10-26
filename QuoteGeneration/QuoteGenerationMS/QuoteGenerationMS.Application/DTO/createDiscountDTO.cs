
namespace QuoteGenerationMS.Application.DTO
{
    public class createDiscountDTO
    {
        public string discount_name { get; set; } = string.Empty;

        public decimal discount_percentage { get; set; }

        public DateTime valid_from { get; set; }

        public DateTime valid_to { get; set; }
    }
}
