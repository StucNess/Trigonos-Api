using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class REACT_TRGNS_RolPaginas : TRGNS_base
    {
        public string Idrol { get; set; }
        public REACT_TRGNS_PaginasWeb? tRGNS_PaginasWeb { get; set; }
        public int Idpagina { get; set; }
        public int? Bhabilitado { get; set; }


    }
}
