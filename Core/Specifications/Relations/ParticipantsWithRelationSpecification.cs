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
        public ParticipantsWithRelationSpecification(ParticipantsParams parametros) : base(x =>
        (string.IsNullOrEmpty(parametros.business_Name) || x.Business_Name == parametros.business_Name)&&
        (string.IsNullOrEmpty(parametros.rut) || x.Rut == parametros.rut))
        {
            AddInclude(p => p.CEN_banks);
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
            AddOrderBy(p => p.ID);
        }

        public ParticipantsWithRelationSpecification(int id, ParticipantsParamsID parametros) : base(x => x.ID == id) 
        
        {   
            AddInclude(p => p.CEN_banks);
            AddOrderBy(p => p.ID);

        }
    }
}
