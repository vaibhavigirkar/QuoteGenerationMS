using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace QuoteGenerationMS.Application.DTO
{
    public class createQuoteDTO
    {
        public int customer_id { get; set; }

        public int product_id { get; set; }

        public int premium_frequency_id { get; set; }

        public int policy_term { get; set; }

        public decimal sum_insured { get; set; }
    }
}


