
using QuoteGenerationMS.Application.DTO;

namespace QuoteGenerationMS.Application.Interface
{
    public interface IUnderwritingClient
    {
        Task<int> CreateCase(createUnderwritingRequestDTO dto);

        Task<List<underwriting_caseResponseDTO>> GetAllCases();

        Task CalculateRiskScore(
            int underwritingId,
            calculateRiskScoreRequestDTO dto);

        Task<underwriting_caseResponseDTO?> GetCaseById(int id);

        Task<risk_scoreResponseDTO?> GetRiskScore(int id);

        Task<underwriting_decisionResponseDTO?> GetDecision(int id);
    }
}
