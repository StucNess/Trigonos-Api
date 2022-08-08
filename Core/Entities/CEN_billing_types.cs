using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_billing_types : TRGNS_base
    {
        public string Natural_Key { get; set; }
        public string Title { get; set; }
        public string Systen_prefix { get; set; }
        public string Description_prefix { get; set; }
        public int Payment_window { get; set; }
        public int Department { get; set; }
        public bool Enable { get; set; }
    }
}
