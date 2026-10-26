
using QuoteGenerationMS.Application.DTO;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Application.Interface
{
    public interface ICustomerRegistrationClient
    {
        Task<customerRegistrationResponseDTO?> GetCustomerById(int customerId);
    }
}
