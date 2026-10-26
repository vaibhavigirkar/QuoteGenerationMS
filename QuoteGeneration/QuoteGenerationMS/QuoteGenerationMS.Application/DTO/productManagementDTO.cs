
namespace QuoteGenerationMS.Application.DTO
{
    public class productManagementDTO
    {
        public int product_id { get; set; }
        public int category_id { get; set; }
        public string product_code { get; set; } = string.Empty;
        public string product_name { get; set; } = string.Empty;
        public string product_type { get; set; } = string.Empty;
        public string? description { get; set; }
        public int minimum_age { get; set; }
        public int maximum_age { get; set; }
        public decimal minimum_sum_insured { get; set; }
        public decimal maximum_sum_insured { get; set; }
        public string status { get; set; } = string.Empty;
    }
}
