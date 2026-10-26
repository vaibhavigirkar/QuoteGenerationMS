using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Domain.Entities
{
    public class quote_rider
    {
        [Key]
        public int rider_selection_id { get; set; }

        // Foreign key to quote within this microservice
        public int quote_id { get; set; }

        // Rider ID owned by Product Management
        public int rider_id { get; set; }

        public decimal rider_premium { get; set; }
    }
}