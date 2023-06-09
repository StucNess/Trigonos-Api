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

        public InstruccionesMetrics(int id, bool? is_creditor, int? est_recepcion)
            : base(x => is_creditor == null ? (x.Creditor == id || x.Debtor == id) && (est_recepcion != null ? x.Estado_recepcion == est_recepcion : x.Estado_recepcion == x.Estado_recepcion) : is_creditor == true ? (x.Creditor == id && (est_recepcion != null ? x.Estado_recepcion == est_recepcion : x.Estado_recepcion == x.Estado_recepcion)) :
            (x.Debtor == id && (est_recepcion != null ? x.Estado_recepcion == est_recepcion : x.Estado_recepcion == x.Estado_recepcion)))
        {

        }

        public InstruccionesMetrics(bool? is_creditor,int id, int? est_facturacion)
            : base(x => is_creditor == null ? (x.Creditor == id || x.Debtor == id) && (est_facturacion != null ? x.Estado_emision == est_facturacion : x.Estado_emision == x.Estado_emision) : is_creditor == true ? (x.Creditor == id && (est_facturacion != null ? x.Estado_emision == est_facturacion : x.Estado_emision == x.Estado_emision)) :
            (x.Debtor == id && (est_facturacion != null ? x.Estado_emision == est_facturacion : x.Estado_emision == x.Estado_emision)))
        {

        }
    }
}
