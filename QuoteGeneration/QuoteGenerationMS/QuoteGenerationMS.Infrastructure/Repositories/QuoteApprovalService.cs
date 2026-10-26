
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class QuoteApprovalService : IQuoteApprovalService
    {
        AppDbContext db;

        public QuoteApprovalService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task ApproveQuote(int id)
        {
            var d = await db.quote.FindAsync(id);

            if (d == null)
                throw new Exception("Quote not found.");

            if (d.quote_status != "Pending")
                throw new Exception("Only Pending quotes can be approved.");

            if (d.expiry_date < DateTime.Today)
            {
                d.quote_status = "Expired";

                await db.quote_history.AddAsync(new quote_history
                {
                    quote_id = d.quote_id,
                    action = "Quote Expired",
                    performed_by = 0,
                    remarks = "Quote expired before approval.",
                    action_date = DateTime.Now
                });

                await db.SaveChangesAsync();

                throw new Exception("Quote has expired.");
            }

            d.quote_status = "Approved";

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = d.quote_id,
                action = "Quote Approved",
                performed_by = 0,
                remarks = "Quote approved.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task RejectQuote(int id)
        {
            var d = await db.quote.FindAsync(id);

            if (d == null)
                throw new Exception("Quote not found.");

            if (d.quote_status != "Pending")
                throw new Exception("Only Pending quotes can be rejected.");

            if (d.expiry_date < DateTime.Today)
            {
                d.quote_status = "Expired";

                await db.quote_history.AddAsync(new quote_history
                {
                    quote_id = d.quote_id,
                    action = "Quote Expired",
                    performed_by = 0,
                    remarks = "Quote expired before rejection.",
                    action_date = DateTime.Now
                });

                await db.SaveChangesAsync();

                throw new Exception("Quote has expired.");
            }

            d.quote_status = "Rejected";

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = d.quote_id,
                action = "Quote Rejected",
                performed_by = 0,
                remarks = "Quote rejected.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }
    }
}
