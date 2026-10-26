
namespace QuoteGenerationMS.Application.DTO
{
    public class createRiskAssessmentDTO
    {
        public int quote_id { get; set; }

        public int age_score { get; set; }

        public int occupation_score { get; set; }

        public int medical_score { get; set; }

        public int lifestyle_score { get; set; }
    }
}
