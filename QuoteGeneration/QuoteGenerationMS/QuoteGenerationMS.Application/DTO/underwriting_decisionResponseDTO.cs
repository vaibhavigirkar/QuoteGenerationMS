namespace QuoteGenerationMS.Application.DTO
{
    public class underwriting_decisionResponseDTO
    {
        public int decision_id { get; set; }
        public int underwriting_id { get; set; }
        public string decision { get; set; } = string.Empty;
        public decimal loading_percentage { get; set; }
        public string? exclusion_details { get; set; }
        public string? rejection_reason { get; set; }
        public int? approved_by { get; set; }
        public DateTime decision_date { get; set; }
    }
}