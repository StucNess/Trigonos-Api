using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class REACT_TRGNS_LogsFacturacioncl:TRGNS_base
    {
        public int? idInstruccion { get; set; }
        public int? idAcreedor { get; set; }
        public string? error { get; set; }
    }
}
