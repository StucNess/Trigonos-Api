﻿namespace TrigonosEnergyWebAPI.DTO
{
    public class MetricInstruccionDto
    {
        public int IDParticipante { get; set; }
        public int TotalNoPagado { get; set; }
        public int TotalPagado { get; set; }
        public int TotalAtrasado { get; set; }
        public int TotalMuestra { get; set; }

    }
}
