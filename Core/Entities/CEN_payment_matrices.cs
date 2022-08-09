using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_payment_matrices : TRGNS_base
    {
        public string? Auxiliary_data { get; set; }
        public string? Created_ts { get; set; }
        public string? Updated_ts { get; set; }  
        public string? Payment_type { get; set; }
        public int? Version { get; set; }
        public string? Payment_file { get; set; }
        public string? Letter_code { get; set; }
        public int? Letter_year { get; set; }
        public string? Letter_file { get; set; }
        public string? Matrix_file { get; set; }
        public string? Publish_date { get; set; }
        public int? Payment_days { get; set; }
        public string? Payment_date { get; set; }
        public string? Billing_date { get; set; }
        public int? Payment_window { get; set; }
        public string? Natural_key { get; set; }
        public string? Reference_code { get; set; }
        public int? billing_window { get; set; }
        public CEN_billing_windows? CEN_billing_windows { get; set; }
        public int? payment_due_type { get; set; }
        public CEN_payment_due_type? CEN_payment_due_type { get; set; }
    }
}
