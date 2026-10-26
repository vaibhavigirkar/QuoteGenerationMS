
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IQuoteHistoryService
    {
        Task<List<quote_historyDTO>> FetchAllHistory();

        Task<List<quote_historyDTO>> FetchHistoryByQuoteId(int quote_id);
    }
}
