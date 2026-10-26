
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        AppDbContext db;
        IMapper mapper;

        public DiscountService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddDiscount(createDiscountDTO dto)
        {
            if (dto.discount_percentage < 0 ||
                dto.discount_percentage > 100)
                throw new Exception("Discount percentage must be between 0 and 100.");

            if (dto.valid_to.Date < dto.valid_from.Date)
                throw new Exception("Valid-to date cannot be earlier than valid-from date.");

            var d = mapper.Map<discount_master>(dto);

            d.discount_id = 0;
            d.status = "Active";

            await db.discount_master.AddAsync(d);
            await db.SaveChangesAsync();
        }

        public async Task<List<discount_masterDTO>> FetchAllDiscounts()
        {
            var d = await db.discount_master.ToListAsync();

            var data = mapper.Map<List<discount_masterDTO>>(d);

            return data;
        }

        public async Task<discount_masterDTO?> FindDiscountById(int id)
        {
            var d = await db.discount_master.FindAsync(id);

            if (d == null)
                return null;

            var data = mapper.Map<discount_masterDTO>(d);

            return data;
        }

        public async Task UpdateDiscount(int id, createDiscountDTO dto)
        {
            var d = await db.discount_master.FindAsync(id);

            if (d == null)
                throw new Exception("Discount not found.");

            if (dto.discount_percentage < 0 ||
                dto.discount_percentage > 100)
                throw new Exception("Discount percentage must be between 0 and 100.");

            if (dto.valid_to.Date < dto.valid_from.Date)
                throw new Exception("Valid-to date cannot be earlier than valid-from date.");

            d.discount_name = dto.discount_name;
            d.discount_percentage = dto.discount_percentage;
            d.valid_from = dto.valid_from;
            d.valid_to = dto.valid_to;

            await db.SaveChangesAsync();
        }

        public async Task DeactivateDiscount(int id)
        {
            var d = await db.discount_master.FindAsync(id);

            if (d == null)
                throw new Exception("Discount not found.");

            d.status = "Inactive";

            await db.SaveChangesAsync();
        }
    }
}
