namespace TrigonosEnergyWebAPI.DTO
{
    public class RegistrarDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? IdEmpresa { get; set; }
        public string? Pais { get; set; }
        public string Password { get; set; }

        public string Rol { get; set; }
        public List<int> ListIdProyects { get; set; }

    }
}
