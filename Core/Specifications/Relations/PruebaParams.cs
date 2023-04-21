using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class PruebaParams : BaseSpecification<REACT_TRGNS_Datos_Facturacion>
    {
        public PruebaParams(int id/*, InstruccionesSpecificationParams productoParams*/)


           : base(x => x.id_instructions == id)

        {
        }
    }
}
