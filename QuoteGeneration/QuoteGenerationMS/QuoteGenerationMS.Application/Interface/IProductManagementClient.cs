
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IProductManagementClient
    {
        Task<productManagementDTO?> GetProductById(int id);
    }
}
