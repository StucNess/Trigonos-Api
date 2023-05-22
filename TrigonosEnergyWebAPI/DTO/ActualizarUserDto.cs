namespace TrigonosEnergyWebAPI.DTO
{
    public class ActualizarUserDto
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int? IdEmpresa { get; set; }
        public string? Pais { get; set; }
        public string? Password { get; set; }
        public string? RolIdAnterior { get; set; }
        public string? RolIdNuevo { get; set; }
        public List<AsignarProyectoDto>? ListDeleteProyecto { get; set; }
        public List<AsignarProyectoDto>? ListNewProyecto { get; set; }

    }
}
