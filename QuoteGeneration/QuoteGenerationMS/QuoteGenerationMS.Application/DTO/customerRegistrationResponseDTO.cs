
namespace QuoteGenerationMS.Application.DTO
{
    public class customerRegistrationResponseDTO
    {
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CustomerStatus { get; set; } = string.Empty;
    }
}
