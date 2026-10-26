namespace QuoteGenerationMS.Application.DTO
{
    public class calculateRiskScoreRequestDTO
    {
        public int age_score { get; set; }
        public int medical_score { get; set; }
        public int occupation_score { get; set; }
        public int lifestyle_score { get; set; }
        public int financial_score { get; set; }
    }
}