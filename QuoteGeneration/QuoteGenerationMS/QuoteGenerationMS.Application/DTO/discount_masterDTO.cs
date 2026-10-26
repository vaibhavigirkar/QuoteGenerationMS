
namespace QuoteGenerationMS.Application.DTO
{
    public class discount_masterDTO
    {
        public int discount_id { get; set; }

        public string discount_name { get; set; } = string.Empty;

        public decimal discount_percentage { get; set; }

        public DateTime valid_from { get; set; }

        public DateTime valid_to { get; set; }

        public string status { get; set; } = string.Empty;
    }
}
