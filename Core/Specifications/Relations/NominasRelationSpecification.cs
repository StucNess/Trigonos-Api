using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class NominasRelationSpecification:BaseSpecification<TRGNS_Datos_Facturacion>
    {


        public NominasRelationSpecification(int id, NominasParamsSpecification parametros)
            : base(x => x.CEN_instruction.Debtor == id &&
            (string.IsNullOrEmpty(parametros.Glosa) || x.CEN_instruction.Payment_matrix_natural_key.Contains(parametros.Glosa)))
        {

            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);

            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction.Participants_creditor);

            AddInclude(p => p.CEN_instruction.Participants_debtor);
            ApplyPaging(parametros.PageSize * (parametros.PageIndex - 1), parametros.PageSize);
        }
    }
}
