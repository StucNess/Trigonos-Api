using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class REACT_TRGNS_PROYECTOS : TRGNS_base
    {

        public int Id_participants { get; set; } 
        public REACT_CEN_Participants? cEN_Participants { get; set; }
        public int? Erp { get; set; }
        public int? Group { get; set; }
        public int? vHabilitado { get; set; }
        public int? Id_nomina_pago { get; set; }


    }
}
