
using System.Net;
using System.Net.Http.Json;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class ProductManagementClient : IProductManagementClient
    {
        private readonly HttpClient _httpClient;

        public ProductManagementClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<productManagementDTO?> GetProductById(int id)
        {
            var response = await _httpClient.GetAsync($"api/v1/products/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<productManagementDTO>();
        }
    }
}
