namespace TrigonosEnergyWebAPI.DTO
{
    public class UsuariosDto
    {
        public string Id { get; set; }
        public string? Email {get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int? IdEmpresa { get; set; }
        public string? Pais { get; set; }

        public string? Role { get; set; }

    }
}
