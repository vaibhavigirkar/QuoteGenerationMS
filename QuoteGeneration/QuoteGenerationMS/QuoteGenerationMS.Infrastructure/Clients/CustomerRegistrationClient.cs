
using System.Net;
using System.Net.Http.Json;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;

namespace QuoteGenerationMS.Infrastructure.Clients
{
    public class CustomerRegistrationClient : ICustomerRegistrationClient
    {
        private readonly HttpClient httpClient;

        public CustomerRegistrationClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<customerRegistrationResponseDTO?> GetCustomerById(
            int customerId)
        {
            var response = await httpClient.GetAsync(
                $"api/v1/customers/{customerId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<customerRegistrationResponseDTO>();
        }
    }
}
