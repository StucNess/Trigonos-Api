using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_instructions : TRGNS_base
    {
        public int Payment_matrix { get; set; }
        public int Creditor { get; set; }
        public int Debtor { get; set; }
        public Int64 Amount { get; set; }
        public Int64 Amount_Gross { get; set; }
        public bool Closed { get; set; }
        public string Status { get; set; }
        public int Status_billed { get; set; }
        public int Status_paid { get; set; }
        public string Resolution { get; set; }
        public string Max_payment_date { get; set; }
        public Int64 Informed_paid_amount { get; set; }
        public bool Is_paid { get; set; }
        public string Payment_matrix_natural_key { get; set; }
        public string Payment_matrix_concept { get; set; }
        public bool Accept_partial_payment { get; set; }
        public string Created_ts { get; set; }
        public string Updated_ts { get; set; }
        public int Trgns_Status_Instructions { get; set; }
    }
}
