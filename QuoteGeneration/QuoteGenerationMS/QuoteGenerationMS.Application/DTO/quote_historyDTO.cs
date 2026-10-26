
namespace QuoteGenerationMS.Application.DTO
{
    public class quote_historyDTO
    {
        public int history_id { get; set; }

        public int quote_id { get; set; }

        public string action { get; set; } = string.Empty;

        public int performed_by { get; set; }

        public string? remarks { get; set; }

        public DateTime action_date { get; set; }
    }
}
