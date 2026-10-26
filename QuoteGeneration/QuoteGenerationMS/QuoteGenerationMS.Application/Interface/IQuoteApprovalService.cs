
namespace QuoteGenerationMS.Application.Interface
{
    public interface IQuoteApprovalService
    {
        Task ApproveQuote(int id);

        Task RejectQuote(int id);
    }
}
