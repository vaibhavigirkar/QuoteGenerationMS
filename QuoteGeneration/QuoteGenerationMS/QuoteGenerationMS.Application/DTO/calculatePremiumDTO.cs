using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuoteGenerationMS.Application.DTO
{
    public class calculatePremiumDTO
    {
        public int quote_id { get; set; }

        public decimal product_rate { get; set; }

        public decimal risk_percentage { get; set; }

        public decimal rider_premium { get; set; }

        public decimal discount_percentage { get; set; }

        public decimal gst_percentage { get; set; }
    }
}
