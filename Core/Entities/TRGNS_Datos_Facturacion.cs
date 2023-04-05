using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TRGNS_Datos_Facturacion : TRGNS_base
    {
        public int? id_instructions { get; set; }
        public CEN_instructions? CEN_instruction { get; set; }
        public int? Estado_emision { get; set; }
        public CEN_billing_status_type? CEN_billing_status_type { get; set; }
        public int? Estado_recepcion { get; set; }
        public TRGNS_dte_reception_status? TRGNS_dte_reception_status { get; set; }
        public int? Estado_pago { get; set; }
        public CEN_payment_status_type? CEN_payment_status_type { get; set; }
        public int? Estado_aceptacion { get; set; }
        public CEN_dte_acceptance_status? CEN_dte_acceptance_status { get; set; }
        public DateTime? Fecha_emision { get; set; }
        public DateTime? Fecha_recepcion { get; set; }
        public DateTime? Fecha_pago { get; set; }
        public DateTime? Fecha_aceptacion { get; set; }
        public int? tipo_instructions { get; set; }
        public int? Folio { get; set; }
        public int? nonconformitie { get; set; }
        public CEN_nonconformities? CEN_nonconformities { get; set; }
    }
}
