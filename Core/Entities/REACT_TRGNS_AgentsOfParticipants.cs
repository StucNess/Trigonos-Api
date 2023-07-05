using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class REACT_TRGNS_AgentsOfParticipants:TRGNS_base
    {
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? RutEmpresa { get; set; }

    }
}
