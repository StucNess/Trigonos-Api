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
            : base
            (x => 
            x.CEN_instruction.Debtor == id &&
            x.Estado_aceptacion == 1 &&
            (string.IsNullOrEmpty(parametros.Glosa) || x.CEN_instruction.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
            ((string.IsNullOrEmpty(parametros.Disc) && string.IsNullOrEmpty(x.CEN_nonconformities.created_ts)) || (!string.IsNullOrEmpty(parametros.Disc) && !string.IsNullOrEmpty(x.CEN_nonconformities.created_ts)))
            
            )
        {

            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_nonconformities);
            AddInclude(p => p.CEN_instruction.Participants_creditor.CEN_banks);
            AddInclude(p => p.CEN_instruction);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices);
            AddInclude(p => p.CEN_instruction.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.CEN_instruction.Participants_creditor);
            AddOrderByDescending(p => p.CEN_nonconformities.created_ts);
            AddInclude(p => p.CEN_instruction.Participants_debtor);
            AddOrderBy(p => p.CEN_nonconformities.created_ts);

        }
    }
}
