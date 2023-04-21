using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class NominasForCountingSpecification : BaseSpecification<REACT_TRGNS_Datos_Facturacion>
    {
        public NominasForCountingSpecification(int id, NominasParamsSpecification parametros)
            : base(x => x.CEN_instruction.Debtor == id &&
            x.Estado_aceptacion == 1 &&
            (string.IsNullOrEmpty(parametros.Glosa) || x.CEN_instruction.Payment_matrix_natural_key.Contains(parametros.Glosa)))
        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_nonconformities);
            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction.Participants_creditor);

            AddInclude(p => p.CEN_instruction.Participants_debtor);
        }
    }
}
