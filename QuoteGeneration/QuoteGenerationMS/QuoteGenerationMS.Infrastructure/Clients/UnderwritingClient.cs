
using System.Net;
using System.Net.Http.Json;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;

namespace QuoteGenerationMS.Infrastructure.Clients
{
    public class UnderwritingClient : IUnderwritingClient
    {
        private readonly HttpClient _httpClient;

        public UnderwritingClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CreateCase(
            createUnderwritingRequestDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/v1/underwriting", dto);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<createUnderwritingResponseDTO>();

            if (result == null || result.underwriting_id <= 0)
                throw new InvalidOperationException(
                    "Underwriting case ID was not returned.");

            return result.underwriting_id;
        }

        public async Task<List<underwriting_caseResponseDTO>> GetAllCases()
        {
            var response = await _httpClient.GetAsync(
                "api/v1/underwriting");

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<List<underwriting_caseResponseDTO>>();

            return result ?? new List<underwriting_caseResponseDTO>();
        }

        public async Task CalculateRiskScore(
            int underwritingId,
            calculateRiskScoreRequestDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"api/v1/underwriting/{underwritingId}/risk-score",
                dto);

            response.EnsureSuccessStatusCode();
        }

        public async Task<underwriting_caseResponseDTO?> GetCaseById(int id)
        {
            var response = await _httpClient.GetAsync(
                $"api/v1/underwriting/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<underwriting_caseResponseDTO>();
        }

        public async Task<risk_scoreResponseDTO?> GetRiskScore(int id)
        {
            var response = await _httpClient.GetAsync(
                $"api/v1/underwriting/{id}/risk-score");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<risk_scoreResponseDTO>();
        }

        public async Task<underwriting_decisionResponseDTO?> GetDecision(int id)
        {
            var response = await _httpClient.GetAsync(
                $"api/v1/underwriting/{id}/decision");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<underwriting_decisionResponseDTO>();
        }
    }
}
