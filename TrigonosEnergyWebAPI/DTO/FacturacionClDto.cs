namespace TrigonosEnergyWebAPI.DTO
{
    public class FacturacionClDto
    {

        public int ID { get; set; }
        public int IdParticipante { get; set; }
        public string? Usuario64 { get; set; }
        public string? RUT64 { get; set; }
        public string? Clave64 { get; set; }
        public string? Puerto64 { get; set; }
        public string? IncluyeLink64 { get; set; }
        public string? UsuarioTest { get; set; }
        public string? ClaveTest { get; set; }
        public string? RutTest { get; set; }
        public bool? Phabilitado { get; set; }
    }
}
