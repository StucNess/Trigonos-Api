using Core.Entities;

namespace TrigonosEnergyWebAPI.DTO
{
    public class ComboBoxAPI
    {
        public IReadOnlyList<CEN_billing_status_type> EstadoFacturacion { get; set; }
        public IReadOnlyList<CEN_dte_acceptance_status> EstadoAceptacion { get; set; }
        public IReadOnlyList<CEN_payment_status_type> EstadoPago { get; set; }

        public IReadOnlyList<TRGNS_dte_reception_status> EstadoRecepcion { get; set; }
        public IReadOnlyList<Concepto> Concepto { get; set; }
    }
}
