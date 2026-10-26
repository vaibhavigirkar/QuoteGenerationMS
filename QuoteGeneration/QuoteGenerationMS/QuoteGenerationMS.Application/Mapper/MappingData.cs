using AutoMapper;
using QuoteGenerationMS.Application.DTO;
using QuoteGenerationMS.Domain.Entities;

namespace QuoteGenerationMS.Application.Mapper
{
    public class MappingData : Profile
    {
        public MappingData()
        {
            CreateMap<quote, quoteDTO>();
            CreateMap<createQuoteDTO, quote>();
            CreateMap<premium_calculation, premiumCalculationDTO>();
            CreateMap<risk_assessment, risk_assessmentDTO>();
            CreateMap<createRiskAssessmentDTO, risk_assessment>();
            CreateMap<discount_master, discount_masterDTO>();
            CreateMap<createDiscountDTO, discount_master>();
            CreateMap<quote_rider, quote_riderDTO>();
            CreateMap<createQuoteRiderDTO, quote_rider>();
            CreateMap<quote_history, quote_historyDTO>();
        }
    }
}