using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class PatchParticipantsRelation : BaseSpecification<REACT_CEN_Participants>
    {
        public PatchParticipantsRelation(int id):base(x=> x.ID == id)
        {

        }
    }
}
