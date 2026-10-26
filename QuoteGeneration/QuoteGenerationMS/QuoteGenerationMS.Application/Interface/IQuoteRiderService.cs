
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IQuoteRiderService
    {
        Task AddRider(createQuoteRiderDTO dto);

        Task<List<quote_riderDTO>> FetchRidersByQuoteId(int quote_id);

        Task RemoveRider(int id);
    }
}
