
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class QuoteHistoryService : IQuoteHistoryService
    {
        AppDbContext db;
        IMapper mapper;

        public QuoteHistoryService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<List<quote_historyDTO>> FetchAllHistory()
        {
            var d = await db.quote_history
                .OrderByDescending(x => x.action_date)
                .ToListAsync();

            var data = mapper.Map<List<quote_historyDTO>>(d);

            return data;
        }

        public async Task<List<quote_historyDTO>> FetchHistoryByQuoteId(
            int quote_id)
        {
            var quote = await db.quote.FindAsync(quote_id);

            if (quote == null)
                throw new Exception("Quote not found.");

            var d = await db.quote_history
                .Where(x => x.quote_id == quote_id)
                .OrderByDescending(x => x.action_date)
                .ToListAsync();

            var data = mapper.Map<List<quote_historyDTO>>(d);

            return data;
        }
    }
}
