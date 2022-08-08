using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_banks : TRGNS_base
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sbif { get; set; }
        public int Type { get; set; }


    }
}
