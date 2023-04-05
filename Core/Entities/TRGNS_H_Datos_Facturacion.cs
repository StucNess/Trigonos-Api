using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TRGNS_H_Datos_Facturacion : TRGNS_base
    {
        public int? id_instruction { get; set; }
        public string? editor { get; set; }
        public DateTime? date { get; set; }
        public int? emission_status_old { get; set; }
        public int? reception_status_old { get; set; }
        public int? payment_status_old { get; set; }
        public int? aceptation_status_old { get; set; }
        public DateTime? emission_date_old { get; set; }
        public DateTime? reception_date_old { get; set; }
        public DateTime? payment_date_old { get; set; }
        public DateTime? aceptation_date_old { get; set; }
        public int? tipo_instruction_old { get; set; }
        public int? folio_old { get; set; }
        public int? emission_status_new { get; set; }
        public int? reception_status_new { get; set; }
        public int? payment_status_new { get; set; }
        public int? aceptation_status_new { get; set; }
        public DateTime? emission_date_new { get; set; }
        public DateTime? reception_date_new { get; set; }
        public DateTime? payment_date_new { get; set; }
        public DateTime? aceptation_date_new { get; set; }
        public int? tipo_instruction_new { get; set; }
        public int? folio_new { get; set; }





    }
}
