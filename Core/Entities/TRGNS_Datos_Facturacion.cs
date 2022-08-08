using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TRGNS_Datos_Facturacion : TRGNS_base
    {
        public int id_instruccions { get; set; }
        public CEN_instructions cEN_Instructions { get; set; }
        public int Estado_emision_id { get; set; }
        public CEN_billing_status_type billing_name { get; set; }
        public int Estado_recepcion_id { get; set; }
        public TRGNS_dte_reception_status Recepcion_name { get; set; }
        public int Estado_pago_id { get; set; }
        public CEN_payment_status_type payment_name { get; set; }
        public int Estado_aceptacion_id { get; set; }
        public CEN_dte_acceptance_status acceptance_name { get; set; }
        public DateTime Fecha_emision { get; set; }
        public DateTime Fecha_recepcion { get; set; }
        public DateTime Fecha_pago { get; set; }
        public DateTime Fecha_aceptacion { get; set; }
        public int Tipo_instruccion { get; set; }
        public int Folio { get; set; }
    }
}
