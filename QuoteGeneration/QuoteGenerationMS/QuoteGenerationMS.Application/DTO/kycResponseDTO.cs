
namespace QuoteGenerationMS.Application.DTO
{
    public class kycResponseDTO
    {
        public int customer_id { get; set; }
        public string kyc_status { get; set; } = string.Empty;
        public bool is_verified { get; set; }
    }
}
