namespace TrigonosEnergyWebAPI.DTO
{
    public class MetricsEstadoFacturadoDto
    {

        public int IDParticipante { get; set; }
        public int TotalNoFacturado { get; set; }
        public int TotalFacturado { get; set; }
        public int TotalFacturadoConAtraso { get; set; }
        public int TotalPendiente { get; set; }
        public int TotalMuestra { get; set; }
    }
}
