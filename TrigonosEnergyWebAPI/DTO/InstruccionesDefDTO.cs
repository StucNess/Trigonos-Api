using Core.Entities;

namespace TrigonosEnergyWebAPI.DTO
{
    public class InstruccionesDefDTO
    {
        public int id_instruccions { get; set; }
        public int Estado_emision { get; set; }
        public string CEN_billing_status_type_name { get; set; }
        public int Estado_recepcion { get; set; }
        public string TRGNS_dte_reception_status_name { get; set; }
        public int Estado_pago { get; set; }
        public string CEN_payment_status_type_name { get; set; }
        public int Estado_aceptacion { get; set; }
        public string CEN_dte_acceptance_status_name { get; set; }
        public DateTime Fecha_emision { get; set; }
        public DateTime Fecha_recepcion { get; set; }
        public DateTime Fecha_pago { get; set; }
        public DateTime Fecha_aceptacion { get; set; }
        public int Tipo_instruccion { get; set; }
        public int Folio { get; set; }
        public string NombreAcreedor { get; set; }
        public int Acreedor { get; set; }
        public string RutAcreedor { get; set; }
        public string NombreDeudor { get; set; }
        public int Deudor { get; set; }
        public string RutDeudor { get; set; }
        public string Glosa { get; set; }
        public string Concepto { get; set; }
        public int MontoNeto { get; set; }
        public int MontoBruto { get; set; }
        public string period { get; set; }
        public string periodEnd { get; set; }
        public string? Carta { get; set; }
        public string? CodigoRef { get; set; }
        public DateTime Fecha_carta { get; set; }

        public string? GiroDeudor { get; set; }
        public string? DireccionDeudor { get; set; }

    }
}
