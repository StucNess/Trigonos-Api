using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class HistorificacionParticipantesSpecification : BaseSpecification<REACT_TRGNS_H_CEN_participants>
    {

        public HistorificacionParticipantesSpecification(int id):base(x => x.id_definir == id)
        {
            AddOrderByDescending(p => p.ID);
        }
        public HistorificacionParticipantesSpecification()
        {
            AddOrderByDescending(p => p.ID);
        }
    }
}
