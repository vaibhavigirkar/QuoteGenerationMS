
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class PremiumCalculationService : IPremiumCalculationService
    {
        AppDbContext db;
        IMapper mapper;

        public PremiumCalculationService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task CalculatePremium(calculatePremiumDTO dto)
        {
            var quote = await db.quote.FindAsync(dto.quote_id);

            if (quote == null)
                throw new Exception("Quote not found.");

            if (quote.quote_status != "Pending")
                throw new Exception("Premium can only be calculated for Pending quotes.");

            if (quote.expiry_date < DateTime.Today)
                throw new Exception("Quote has expired.");

            if (dto.product_rate <= 0)
                throw new Exception("Product rate must be greater than zero.");

            if (dto.risk_percentage < 0 ||
                dto.discount_percentage < 0 ||
                dto.discount_percentage > 100 ||
                dto.rider_premium < 0 ||
                dto.gst_percentage < 0)
                throw new Exception("Invalid premium calculation values.");

            decimal basePremium =
                quote.sum_insured * dto.product_rate / 1000m;

            decimal riskLoading =
                basePremium * dto.risk_percentage / 100m;

            decimal discountAmount =
                basePremium * dto.discount_percentage / 100m;

            decimal taxablePremium =
                basePremium + riskLoading + dto.rider_premium - discountAmount;

            if (taxablePremium < 0)
                throw new Exception("Taxable premium cannot be negative.");

            decimal gstAmount =
                taxablePremium * dto.gst_percentage / 100m;

            decimal finalPremium =
                taxablePremium + gstAmount;

            var calculation = new premium_calculation
            {
                quote_id = dto.quote_id,
                base_premium = Math.Round(basePremium, 2),
                risk_loading = Math.Round(riskLoading, 2),
                rider_premium = Math.Round(dto.rider_premium, 2),
                discount_amount = Math.Round(discountAmount, 2),
                gst_amount = Math.Round(gstAmount, 2),
                final_premium = Math.Round(finalPremium, 2)
            };

            await db.premium_calculation.AddAsync(calculation);
            await db.SaveChangesAsync();

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quote.quote_id,
                action = "Premium Calculated",
                performed_by = 0,
                remarks = "Premium calculation completed.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task<premiumCalculationDTO?> GetPremiumByQuoteId(
            int quote_id)
        {
            var d = await db.premium_calculation
                .Where(x => x.quote_id == quote_id)
                .OrderByDescending(x => x.calculation_id)
                .FirstOrDefaultAsync();

            if (d == null)
                return null;

            var data = mapper.Map<premiumCalculationDTO>(d);

            return data;
        }
    }
}
