using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class ParticipantsWithRelationSpecification : BaseSpecification<CEN_Participants>
    {
        public ParticipantsWithRelationSpecification(ParticipantsParams parametros)
        {
            AddInclude(p => p.CEN_banks);
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
        }

        public ParticipantsWithRelationSpecification(int id) : base(x => x.ID == id)
        {
            AddInclude(p => p.CEN_banks);
           
        }
    }
}
