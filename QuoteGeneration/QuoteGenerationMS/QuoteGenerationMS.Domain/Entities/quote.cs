using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Domain.Entities
{
    public class quote
    {
        [Key]
        public int quote_id { get; set; }

        public string quote_number { get; set; } = string.Empty;

        // IDs owned by other microservices
        public int customer_id { get; set; }
        public int product_id { get; set; }
        public int premium_frequency_id { get; set; }

        public int policy_term { get; set; }

        public decimal sum_insured { get; set; }

        public DateTime quote_date { get; set; }

        public DateTime expiry_date { get; set; }

        // Pending, Approved, Rejected, Expired, Converted
        public string quote_status { get; set; } = "Pending";
    }
}
