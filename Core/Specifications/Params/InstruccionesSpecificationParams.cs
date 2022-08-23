using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class InstruccionesSpecificationParams
    {
        public int participant;
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public string? Glosa { get; set; }
        public string? Concepto { get; set; }
        public string? EstadoAceptacion { get; set; }
        public string? EstadoRecepcion { get; set; }
        public int? Acreedor { get; set; }
        public int? Deudor { get; set; }
        public int? MontoNeto { get; set; }
        public int? MontoBruto { get; set; }
        public string? EstadoEmision { get; set; }
        public string? EstadoPago { get; set; }
        public string? RutAcreedor { get; set; }
        public string? RutDeudor { get; set; }
        public int? Folio { get; set; }
        public string? NombreAcreedor { get; set; }
        public string? NombreDeudor { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 100;
        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
