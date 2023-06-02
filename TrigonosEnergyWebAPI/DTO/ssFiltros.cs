using Core.Entities;

namespace TrigonosEnergyWebAPI.DTO
{
    public class ssFiltros
    {

        public IReadOnlyList<probandoMapper> label { get; set; }
        public IReadOnlyList<probandoMapper> Carta { get; set; }
        public IReadOnlyList<probandoMapper> CodRef { get; set; }

    }
}
