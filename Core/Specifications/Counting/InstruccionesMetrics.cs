using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class InstruccionesMetrics: BaseSpecification<REACT_CEN_instructions_Def>
    {
        public InstruccionesMetrics(int id, int? est_pago, bool? is_creditor)
            : base(x => is_creditor == null ? (x.Creditor == id || x.Debtor == id) && (est_pago != null ? x.Estado_pago == est_pago : x.Estado_pago == x.Estado_pago) : is_creditor ==true? (x.Creditor == id && ( est_pago !=null? x.Estado_pago == est_pago : x.Estado_pago == x.Estado_pago)):
            (x.Debtor == id && (est_pago != null ? x.Estado_pago == est_pago : x.Estado_pago == x.Estado_pago)))
        {
           
        }
    }
}
