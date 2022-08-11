using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class InstruccionesSpecificationParams
    {
        public int participant;
        public string? EstadoAceptacion { get; set; }
        public string? EstadoRecepcion { get; set; }
        public string? EstadoEmision { get; set; }
        public string? EstadoPago { get; set; }
        public int? Folio { get; set; }
        public string? NombreAcreedor { get; set; }
        public string? NombreDeudor { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 100;
        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
