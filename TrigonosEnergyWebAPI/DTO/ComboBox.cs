namespace TrigonosEnergyWebAPI.DTO
{
    public class ComboBox<T> where T : class
    {
        public IReadOnlyList<T> EstadoAceptacion { get; set; }
        public IReadOnlyList<T> EstadoPago { get; set; }
        public IReadOnlyList<T> EstadoFacturacion { get; set; }
        public IReadOnlyList<T> EstadoRecepcion { get; set; }
    }
}
