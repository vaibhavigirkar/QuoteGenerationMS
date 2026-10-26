
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IPremiumCalculationService
    {
        Task CalculatePremium(calculatePremiumDTO dto);

        Task<premiumCalculationDTO?> GetPremiumByQuoteId(int quote_id);
    }
}
