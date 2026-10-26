
namespace QuoteGenerationMS.Application.Interface
{
    public interface IQuoteLifecycleService
    {
        Task ExpireQuotes();

        Task ConvertQuote(int id);
    }
}
