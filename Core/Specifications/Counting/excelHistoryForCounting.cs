using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class excelHistoryForCounting : BaseSpecification<REACT_TRGNS_Excel_History>
    {
        public excelHistoryForCounting(excelHistoryParams parametros) : base (x =>
        !parametros.id.HasValue || x.idParticipant == parametros.id)
        {
         
        }
    }
}
