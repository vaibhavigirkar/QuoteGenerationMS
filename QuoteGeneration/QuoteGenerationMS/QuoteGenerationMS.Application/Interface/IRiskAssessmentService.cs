
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IRiskAssessmentService
    {
        Task CreateRiskAssessment(createRiskAssessmentDTO dto);

        Task<risk_assessmentDTO?> GetRiskAssessmentByQuoteId(int quote_id);
    }
}
