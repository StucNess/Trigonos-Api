﻿using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Counting
{
    public class NominasForCountingSpecification : BaseSpecification<REACT_CEN_instructions_Def>
    {
        public NominasForCountingSpecification(int id, NominasParamsSpecification parametros)
              : base
            (x =>
            x.Debtor == id &&
            x.Estado_emision == 2 &&
            (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
            (string.IsNullOrEmpty(parametros.Disc) || (!string.IsNullOrEmpty(parametros.Disc) && !string.IsNullOrEmpty(x.CEN_nonconformities.created_ts))))

        {
            AddInclude(p => p.CEN_dte_acceptance_status);
            AddInclude(p => p.TRGNS_dte_reception_status);
            AddInclude(p => p.CEN_payment_status_type);
            AddInclude(p => p.CEN_billing_status_type);
            AddInclude(p => p.CEN_nonconformities);
            AddInclude(p => p.cEN_Payment_Matrices);
            AddInclude(p => p.cEN_Payment_Matrices.CEN_billing_windows);
            AddInclude(p => p.Participants_creditor);

            AddInclude(p => p.Participants_debtor);
        }
    }
}
