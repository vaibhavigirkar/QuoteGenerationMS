using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteGenerationMS.Domain.Entities
{
    public class discount_master
    {
        [Key]
        public int discount_id { get; set; }

        public string discount_name { get; set; } = string.Empty;

        public decimal discount_percentage { get; set; }

        public DateTime valid_from { get; set; }

        public DateTime valid_to { get; set; }

        // Active, Inactive
        public string status { get; set; } = "Active";
    }
}
