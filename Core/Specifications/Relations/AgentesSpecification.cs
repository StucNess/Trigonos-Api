using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class AgentesSpecification : BaseSpecification<REACT_TRGNS_AgentsOfParticipants>
    {
        public AgentesSpecification(AgentesSpecificationParams agentesParams)
             : base(x => 
            (string.IsNullOrEmpty(agentesParams.rutEmpresa) || x.RutEmpresa == agentesParams.rutEmpresa)
          

            )
        {
           
            ApplyPaging(agentesParams.PageSize * (agentesParams.PageIndex - 1), agentesParams.PageSize);
            AddOrderByDescending(p => p.ID);
        }
        public AgentesSpecification(string rutEmpresa)
           : base(x =>
          (string.IsNullOrEmpty(rutEmpresa) || x.RutEmpresa == rutEmpresa)


          )
        {
          
        }
        public AgentesSpecification()
        {
          
        }

       
    }
}
