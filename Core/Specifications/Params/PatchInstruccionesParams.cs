using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class PatchInstruccionesParams
    {
        public int? EstadoEmision { get; set; }
        public int? EstadoRecepcion { get; set; }
        public int? EstadoPago { get; set; }
        public int? EstadoAceptacion { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public int? TipoInstructions { get; set; }
        public int? Folio { get; set; }
    }
}
