using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class InstruccionesRelationSpecification : BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public InstruccionesRelationSpecification(int id, InstruccionesSpecificationParams productoParams)

            : base(x => x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id)
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            AddInclude(p => p.CEN_instruction.Participants_debtor);
            ApplyPaging(productoParams.PageSize * (productoParams.PageIndex - 1), productoParams.PageSize);


        }
    }
}
