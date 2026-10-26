
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class QuoteRiderService : IQuoteRiderService
    {
        AppDbContext db;
        IMapper mapper;

        public QuoteRiderService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddRider(createQuoteRiderDTO dto)
        {
            var quote = await db.quote.FindAsync(dto.quote_id);

            if (quote == null)
                throw new Exception("Quote not found.");

            if (quote.quote_status != "Pending")
                throw new Exception("Riders can only be added to Pending quotes.");

            if (quote.expiry_date < DateTime.Today)
                throw new Exception("Quote has expired.");

            if (dto.rider_id <= 0)
                throw new Exception("Invalid rider ID.");

            if (dto.rider_premium < 0)
                throw new Exception("Rider premium cannot be negative.");

            var duplicate = await db.quote_rider.AnyAsync(
                x => x.quote_id == dto.quote_id &&
                     x.rider_id == dto.rider_id);

            if (duplicate)
                throw new Exception("This rider is already selected for the quote.");

            var d = mapper.Map<quote_rider>(dto);

            d.rider_selection_id = 0;

            await db.quote_rider.AddAsync(d);
            await db.SaveChangesAsync();

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quote.quote_id,
                action = "Rider Added",
                performed_by = 0,
                remarks = $"Rider ID {dto.rider_id} added.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task<List<quote_riderDTO>> FetchRidersByQuoteId(
            int quote_id)
        {
            var d = await db.quote_rider
                .Where(x => x.quote_id == quote_id)
                .ToListAsync();

            var data = mapper.Map<List<quote_riderDTO>>(d);

            return data;
        }

        public async Task RemoveRider(int id)
        {
            var d = await db.quote_rider.FindAsync(id);

            if (d == null)
                throw new Exception("Selected rider not found.");

            var quote = await db.quote.FindAsync(d.quote_id);

            if (quote == null)
                throw new Exception("Quote not found.");

            if (quote.quote_status != "Pending")
                throw new Exception("Riders can only be removed from Pending quotes.");

            if (quote.expiry_date < DateTime.Today)
                throw new Exception("Quote has expired.");

            db.quote_rider.Remove(d);
            await db.SaveChangesAsync();

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quote.quote_id,
                action = "Rider Removed",
                performed_by = 0,
                remarks = $"Rider ID {d.rider_id} removed.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }
    }
}
