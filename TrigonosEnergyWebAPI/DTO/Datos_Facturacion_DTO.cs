namespace TrigonosEnergyWebAPI.DTO
{
    public class Datos_Facturacion_DTO
    {
        public int? id_instructions { get; set; }
        public int? Estado_emision { get; set; }
        public int? Estado_recepcion { get; set; }
        public int? Estado_pago { get; set; }
        public int? Estado_aceptacion { get; set; }
        public DateTime? Fecha_emision { get; set; }
        public DateTime? Fecha_recepcion { get; set; }
        public DateTime? Fecha_pago { get; set; }
        public DateTime? Fecha_aceptacion { get; set; }
        public int? tipo_instructions { get; set; }
        public int? Folio { get; set; }
    }
}
