using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ParticipantsWithRelationSpecification : BaseSpecification<CEN_Participants>
    {
        public ParticipantsWithRelationSpecification()
        {
            AddInclude(p => p.CEN_banks);
        }

        public ParticipantsWithRelationSpecification(int id) : base(x => x.ID == id)
        {
            AddInclude(p => p.CEN_banks);
        }
    }
}
