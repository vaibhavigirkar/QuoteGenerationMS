
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Repositories
{
    public class QuoteService : IQuoteService
    {
        private readonly AppDbContext db;
        private readonly IMapper mapper;
        private readonly IProductManagementClient productClient;
        private readonly IUnderwritingClient underwritingClient;
        private readonly ICustomerRegistrationClient customerClient;

        public QuoteService(
            AppDbContext db,
            IMapper mapper,
            IProductManagementClient productClient,
            IUnderwritingClient underwritingClient,
            ICustomerRegistrationClient customerClient)
        {
            this.db = db;
            this.mapper = mapper;
            this.productClient = productClient;
            this.underwritingClient = underwritingClient;
            this.customerClient = customerClient;
        }

        public async Task CreateQuote(createQuoteDTO dto)
        {
            // 1. Validate customer through Customer Registration MS
            if (dto.customer_id <= 0)
                throw new ArgumentException("Invalid customer ID.");

            var customer =
                await customerClient.GetCustomerById(dto.customer_id);

            if (customer == null)
                throw new KeyNotFoundException(
                    $"Customer with ID {dto.customer_id} was not found.");

            if (!string.Equals(
                    customer.CustomerStatus,
                    "Active",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Only active customers can create quotes.");
            }

            // 2. Validate product through Product Management MS
            var product =
                await productClient.GetProductById(dto.product_id);

            if (product == null)
                throw new Exception(
                    "Product not found in Product Management.");

            if (!string.Equals(
                    product.status,
                    "Active",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Cannot create quote because the product is inactive.");
            }

            if (dto.sum_insured < product.minimum_sum_insured ||
                dto.sum_insured > product.maximum_sum_insured)
            {
                throw new ArgumentException(
                    $"Sum insured must be between " +
                    $"{product.minimum_sum_insured} and " +
                    $"{product.maximum_sum_insured}.");
            }

            // 3. Create quote
            var d = mapper.Map<quote>(dto);

            d.quote_id = 0;
            d.quote_number =
                $"QT-{Guid.NewGuid().ToString("N")[..12].ToUpper()}";
            d.quote_date = DateTime.Today;
            d.expiry_date = DateTime.Today.AddDays(30);
            d.quote_status = "Pending";

            await db.quote.AddAsync(d);
            await db.SaveChangesAsync();

            // 4. Save quote history
            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = d.quote_id,
                action = "Quote Created",
                performed_by = 0,
                remarks =
                    $"Quote created for customer {dto.customer_id} " +
                    "with Pending status.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task<int> SendQuoteForUnderwriting(
            int quoteId, int customerId)
        {
            var quote = await db.quote.FindAsync(quoteId);

            if (quote == null)
                throw new KeyNotFoundException("Quote not found.");

            if (quote.quote_status != "Pending")
                throw new InvalidOperationException(
                    "Only Pending quotes can be sent for underwriting.");

            if (customerId <= 0)
                throw new ArgumentException("Invalid customer ID.");

            // Verify the customer before creating an underwriting case.
            var customer =
                await customerClient.GetCustomerById(customerId);

            if (customer == null)
                throw new KeyNotFoundException(
                    $"Customer with ID {customerId} was not found.");

            if (!string.Equals(
                    customer.CustomerStatus,
                    "Active",
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Only active customers can be sent for underwriting.");
            }

            if (quote.customer_id != customerId)
                throw new InvalidOperationException(
                    "Customer ID does not match the quote.");

            var request = new createUnderwritingRequestDTO
            {
                quote_id = quote.quote_id,
                customer_id = customerId,
                product_id = quote.product_id
            };

            var underwritingId =
                await underwritingClient.CreateCase(request);

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quoteId,
                action = "Underwriting Case Created",
                performed_by = 0,
                remarks =
                    $"Underwriting case created. ID: {underwritingId}",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();

            return underwritingId;
        }

        private async Task<underwriting_caseResponseDTO>
            FindUnderwritingCase(int quoteId)
        {
            var quote = await db.quote.FindAsync(quoteId);

            if (quote == null)
                throw new KeyNotFoundException("Quote not found.");

            var cases = await underwritingClient.GetAllCases();

            var result = cases.FirstOrDefault(
                x => x.quote_id == quoteId);

            if (result == null)
                throw new KeyNotFoundException(
                    "No underwriting case exists for this quote.");

            return result;
        }

        public async Task CalculateUnderwritingRiskScore(
            int quoteId, calculateRiskScoreRequestDTO dto)
        {
            var underwritingCase =
                await FindUnderwritingCase(quoteId);

            if (underwritingCase.status == "Approved" ||
                underwritingCase.status == "Rejected")
            {
                throw new InvalidOperationException(
                    "Risk score cannot be updated after a final decision.");
            }

            await underwritingClient.CalculateRiskScore(
                underwritingCase.underwriting_id, dto);

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quoteId,
                action = "Underwriting Risk Score Calculated",
                performed_by = 0,
                remarks =
                    $"Risk score requested for case " +
                    $"{underwritingCase.underwriting_id}.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task<underwriting_caseResponseDTO?>
            GetUnderwritingCase(int quoteId)
        {
            var item = await FindUnderwritingCase(quoteId);

            return await underwritingClient.GetCaseById(
                item.underwriting_id);
        }

        public async Task<risk_scoreResponseDTO?>
            GetUnderwritingRiskScore(int quoteId)
        {
            var item = await FindUnderwritingCase(quoteId);

            return await underwritingClient.GetRiskScore(
                item.underwriting_id);
        }

        public async Task<underwriting_decisionResponseDTO?>
            GetUnderwritingDecision(int quoteId)
        {
            var item = await FindUnderwritingCase(quoteId);

            return await underwritingClient.GetDecision(
                item.underwriting_id);
        }

        public async Task<List<quoteDTO>> GetAllQuotes()
        {
            var data = await db.quote.ToListAsync();
            return mapper.Map<List<quoteDTO>>(data);
        }

        public async Task<quoteDTO?> GetQuoteById(int id)
        {
            var data = await db.quote.FindAsync(id);

            return data == null ? null : mapper.Map<quoteDTO>(data);
        }

        public async Task UpdateQuote(int id, updateQuoteDTO dto)
        {
            var data = await db.quote.FindAsync(id);

            if (data == null)
                throw new KeyNotFoundException("Quote not found.");

            if (data.quote_status != "Pending")
                throw new InvalidOperationException(
                    "Only Pending quotes can be updated.");

            if (data.expiry_date < DateTime.Today)
            {
                data.quote_status = "Expired";

                await db.quote_history.AddAsync(new quote_history
                {
                    quote_id = data.quote_id,
                    action = "Quote Expired",
                    performed_by = 0,
                    remarks = "Quote expired.",
                    action_date = DateTime.Now
                });

                await db.SaveChangesAsync();

                throw new InvalidOperationException("Quote has expired.");
            }

            data.premium_frequency_id = dto.premium_frequency_id;
            data.policy_term = dto.policy_term;
            data.sum_insured = dto.sum_insured;

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = data.quote_id,
                action = "Quote Updated",
                performed_by = 0,
                remarks = "Quote details updated.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task CancelQuote(int id)
        {
            var data = await db.quote.FindAsync(id);

            if (data == null)
                throw new KeyNotFoundException("Quote not found.");

            if (data.quote_status == "Converted")
                throw new InvalidOperationException(
                    "Converted quote cannot be cancelled.");

            if (data.quote_status == "Expired" ||
                data.quote_status == "Rejected")
            {
                throw new InvalidOperationException(
                    "Expired or rejected quote cannot be cancelled.");
            }

            data.quote_status = "Rejected";

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = data.quote_id,
                action = "Quote Cancelled",
                performed_by = 0,
                remarks = "Quote cancelled.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }
    }
}
