using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class LogsFacturacionclRelationSpecification : BaseSpecification<REACT_TRGNS_LogsFacturacioncl>
    {
        public LogsFacturacionclRelationSpecification(int id, LogsFacturacionParams parametros)
          : base
          (x =>
          x.idAcreedor == id     
          )
        {
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
        }
    }
}
