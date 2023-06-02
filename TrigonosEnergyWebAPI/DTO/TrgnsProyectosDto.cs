using Core.Entities;

namespace TrigonosEnergyWebAPI.DTO
{
    public class TrgnsProyectosDto
    {
        public int ID { get; set; }
        public int Id_participants { get; set; }
        public int Erp { get; set; }

        public int vHabilitado { get; set; }
        public int Id_nomina_pago { get; set; }
    }
}
