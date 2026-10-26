using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Domain.Entities
{
    public class risk_assessment
    {
        [Key]
        public int risk_id { get; set; }

        // Foreign key to quote within this microservice
        public int quote_id { get; set; }

        public int age_score { get; set; }

        public int occupation_score { get; set; }

        public int medical_score { get; set; }

        public int lifestyle_score { get; set; }

        public int total_score { get; set; }

        // Low, Medium, High
        public string risk_category { get; set; } = "Low";
    }
}
