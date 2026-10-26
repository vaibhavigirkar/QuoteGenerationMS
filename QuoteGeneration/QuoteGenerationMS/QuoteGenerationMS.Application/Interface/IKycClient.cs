
using QuoteGenerationMS.Application.DTO;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IKycClient
    {
        Task<kycResponseDTO?> GetKycStatus(int customerId);
    }
}
