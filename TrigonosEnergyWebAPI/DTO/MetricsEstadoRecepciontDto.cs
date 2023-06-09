namespace TrigonosEnergyWebAPI.DTO
{
    public class MetricsEstadoRecepciontDto
    {
        public int IDParticipante { get; set; }
        public int TotalRecepcionado { get; set; }
        public int TotalNoRecepcionado { get; set; }
        public int TotalRechazado { get; set; }
        public int TotalMuestra { get; set; }

    }
}
