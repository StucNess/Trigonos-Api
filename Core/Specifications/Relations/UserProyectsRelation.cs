using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class UserProyectsRelation : BaseSpecification<REACT_TRGNS_UserProyects>
    {
        public UserProyectsRelation(string id):base(x=> x.idUser == id )
        {
            AddInclude(p => p.cEN_Participants);
            AddInclude(p => p.cEN_Participants.CEN_banks);
        }
    }
}
