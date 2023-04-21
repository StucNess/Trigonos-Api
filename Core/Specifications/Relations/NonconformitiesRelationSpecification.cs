using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class NonconformitiesRelationSpecification : BaseSpecification<REACT_CEN_nonconformities>
    {
        public NonconformitiesRelationSpecification(NonconformitiesSpecificationParams nonconformitiesParams)
           
        {
            ApplyPaging(nonconformitiesParams.PageSize * (nonconformitiesParams.PageIndex - 1), nonconformitiesParams.PageSize);
        }
    }
}
