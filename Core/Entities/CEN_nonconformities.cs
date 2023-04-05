using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CEN_nonconformities:TRGNS_base
    {
        public string? auxiliary_data { get; set; }
        public string? created_ts { get; set; }
        public string? updated_ts { get; set; }
        public bool? is_open { get; set; }
        public string? description { get; set; }
        public string? file { get; set; }
        public bool? reported_by_creditor { get; set; }
        public string? historial_file { get; set; }
        public string? closed_date { get; set; }
        public string? jira_key { get; set; }
        public string? process_rad_number { get; set; }

        public int? id_case { get; set; }
        public int? instruction { get; set; }
        public CEN_instructions? CEN_Instructions { get; set; }
        public int? reason { get; set; }
    }
}
