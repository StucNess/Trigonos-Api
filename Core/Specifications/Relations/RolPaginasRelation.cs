using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class RolPaginasRelation : BaseSpecification<REACT_TRGNS_RolPaginas>
    {
        public RolPaginasRelation() 
        {
            AddInclude(p => p.tRGNS_PaginasWeb);
            AddInclude(p => p.rolAspNet);

        }
    }
}
