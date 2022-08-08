using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_billing_windows : TRGNS_base
    {
        public string Natural_key { get; set; }
        public int Id_billing_Type { get; set; }
        public CEN_billing_types Billing_Types { get; set; }
        public string Periods { get; set; }
        public string created_ts { get; set; }
        public string updated_ts { get; set; }

    }
}
