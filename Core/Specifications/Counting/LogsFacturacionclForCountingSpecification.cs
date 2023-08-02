using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class LogsFacturacionclForCountingSpecification : BaseSpecification<REACT_TRGNS_LogsFacturacioncl>
    {
                public LogsFacturacionclForCountingSpecification(int id, LogsFacturacionParams parametros)
          : base
          (x =>
          x.idAcreedor == id     
          )
        {
         
        }
    }
}
