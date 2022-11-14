using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_billing_windows : TRGNS_base
    {
        public string? natural_key { get; set; }
        
        public int? billing_type { get; set; }
        public CEN_billing_types? Billing_Types { get; set; }
        public string? periods { get; set; }
        public string? created_ts { get; set; }
        public string? updated_ts { get; set; }
        public DateTime? period { get; set; }
        public DateTime? period_end { get; set; }
    }
}
