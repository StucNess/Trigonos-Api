using Core.Entities;

namespace TrigonosEnergyWebAPI.DTO
{
    public class ssFiltros
    {

        public IReadOnlyList<ConceptoMapper> label { get; set; }
        public IReadOnlyList<CartaMapper> Carta { get; set; }
        public IReadOnlyList<CodRefMapper> CodRef { get; set; }

    }
}
