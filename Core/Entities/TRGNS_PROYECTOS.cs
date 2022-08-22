using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TRGNS_PROYECTOS : TRGNS_base
    {

        public int Id_participants { get; set; } 
        public CEN_Participants? cEN_Participants { get; set; }
        public int vHabilitado { get; set; }
    }
}
