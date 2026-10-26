
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Application.Interface;
using QuoteGenerationMS.Domain.Entities;
using QuoteGenerationMS.Infrastructure.Data;

namespace QuoteGenerationMS.Infrastructure.Services
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        AppDbContext db;
        IMapper mapper;

        public RiskAssessmentService(AppDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task CreateRiskAssessment(createRiskAssessmentDTO dto)
        {
            var quote = await db.quote.FindAsync(dto.quote_id);

            if (quote == null)
                throw new Exception("Quote not found.");

            if (quote.quote_status != "Pending")
                throw new Exception("Risk assessment is allowed only for Pending quotes.");

            if (dto.age_score < 0 ||
                dto.occupation_score < 0 ||
                dto.medical_score < 0 ||
                dto.lifestyle_score < 0)
                throw new Exception("Risk scores cannot be negative.");

            var totalScore =
                dto.age_score +
                dto.occupation_score +
                dto.medical_score +
                dto.lifestyle_score;

            string category;

            if (totalScore <= 30)
                category = "Low";
            else if (totalScore <= 60)
                category = "Medium";
            else
                category = "High";

            var existing = await db.risk_assessment
                .FirstOrDefaultAsync(x => x.quote_id == dto.quote_id);

            if (existing == null)
            {
                var d = mapper.Map<risk_assessment>(dto);

                d.total_score = totalScore;
                d.risk_category = category;

                await db.risk_assessment.AddAsync(d);
            }
            else
            {
                existing.age_score = dto.age_score;
                existing.occupation_score = dto.occupation_score;
                existing.medical_score = dto.medical_score;
                existing.lifestyle_score = dto.lifestyle_score;
                existing.total_score = totalScore;
                existing.risk_category = category;
            }

            await db.SaveChangesAsync();

            await db.quote_history.AddAsync(new quote_history
            {
                quote_id = quote.quote_id,
                action = "Risk Assessment Completed",
                performed_by = 0,
                remarks = $"Risk category: {category}; total score: {totalScore}.",
                action_date = DateTime.Now
            });

            await db.SaveChangesAsync();
        }

        public async Task<risk_assessmentDTO?> GetRiskAssessmentByQuoteId(
            int quote_id)
        {
            var d = await db.risk_assessment
                .FirstOrDefaultAsync(x => x.quote_id == quote_id);

            if (d == null)
                return null;

            var data = mapper.Map<risk_assessmentDTO>(d);

            return data;
        }
    }
}
