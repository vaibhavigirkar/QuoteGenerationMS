
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IDiscountService
    {
        Task AddDiscount(createDiscountDTO dto);

        Task<List<discount_masterDTO>> FetchAllDiscounts();

        Task<discount_masterDTO?> FindDiscountById(int id);

        Task UpdateDiscount(int id, createDiscountDTO dto);

        Task DeactivateDiscount(int id);
    }
}
