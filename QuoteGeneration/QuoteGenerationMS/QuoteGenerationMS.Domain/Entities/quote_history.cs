using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Domain.Entities
{
    public class quote_history
    {
        [Key]
        public int history_id { get; set; }

        // Foreign key to quote within this microservice
        public int quote_id { get; set; }

        public string action { get; set; } = string.Empty;

        // User/employee ID from the relevant service
        public int performed_by { get; set; }

        public string? remarks { get; set; }

        public DateTime action_date { get; set; }
    }
}
