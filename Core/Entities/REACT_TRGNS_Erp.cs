using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class REACT_TRGNS_Erp : TRGNS_base
    {

        public string? nombreErp { get; set; }
        public int? bhabilitado { get; set; }

        public int? estado_trgns { get; set; }
        public string? iDFacturador { get; set; }
    
    }
}
