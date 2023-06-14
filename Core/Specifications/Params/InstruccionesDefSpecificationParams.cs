using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Params
{
    public class InstruccionesDefSpecificationParams
    {
        public int participant;
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaAceptacion { get; set; }
        public DateTime? InicioPeriodo { get; set; }
        public DateTime? TerminoPeriodo { get; set; }
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
        public string? conFolio { get; set; }
        public int? Folio { get; set; }
        public string? NombreAcreedor { get; set; }
        public string? NombreDeudor { get; set; }
        public string? Carta { get; set; }
        public string? CodigoRef { get; set; }
        public string? OrderByNeto { get; set; }
        public string? OrderByBruto { get; set; }
        public string? OrderByFechaEmision { get; set; }
        public string? OrderByFechaPago { get; set; }
        public string? OrderByFechaCarta { get; set; }
        public string? OrderByFolio { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 5000;
        private int _pageSize = 100;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
