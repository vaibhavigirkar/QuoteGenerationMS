
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class QuoteLifecycleService : IQuoteLifecycleService
    {
        AppDbContext db;

        public QuoteLifecycleService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task ExpireQuotes()
        {
            var today = DateTime.Today;

            var quotes = await db.quote
                .Where(x => x.expiry_date < today &&
                    (x.quote_status == "Pending" ||
                     x.quote_status == "Approved"))
                .ToListAsync();

            foreach (var d in quotes)
            {
                d.quote_status = "Expired";

                await db.quote_history.AddAsync(new quote_history
                {
                    quote_id = d.quote_id,
                    action = "Quote Expired",
                    performed_by = 0,
                    remarks = "Quote validity period has ended.",
                    action_date = DateTime.Now
                });
            }

            await db.SaveChangesAsync();
        }

        public async Task ConvertQuote(int id)
        {
            var d = await db.quote.FindAsync(id);

            if (d == null)
                throw new Exception("Quote not found.");

            if (d.expiry_date < DateTime.Today)
            {
                if (d.quote_status == "Pending" ||
                    d.quote_status == "Approved")
                {
                    d.quote_status = "Expired";

                    await db.quote_history.AddAsync(new quote_history
                    {
                        quote_id = d.quote_id,
                        action = "Quote Expired",
                        performed_by = 0,
                        remarks = "Quote expired before conversion.",
                        action_date = DateTime.Now
                    });

                    await db.SaveChangesAsync();
                }

                throw new Exception("Expired quotes cannot be converted.");
            }

            if (d.quote_status != "Approved")
                throw new Exception("Only Approved quotes can be converted.");

            // Policy issuance, KYC verification, and payment
            // validation will be connected in the integration phase.
            throw new InvalidOperationException(
                "Conversion is pending integration with KYC, payment, and policy issuance services.");
        }
    }
}
