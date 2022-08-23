using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class ProyectosRelation: BaseSpecification<TRGNS_PROYECTOS>
    {
    
        public ProyectosRelation():base(x=> x.vHabilitado == 1)
        {
            AddInclude(p => p.cEN_Participants);
            AddInclude(p => p.cEN_Participants.CEN_banks);
        }
    }
}
