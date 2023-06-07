using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class excelHistorySpecification : BaseSpecification<REACT_TRGNS_Excel_History>
    {
        public excelHistorySpecification(excelHistoryParams parametros):base(x=>
        !parametros.id.HasValue || x.idParticipant == parametros.id)
        {
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
        }
    }
}
