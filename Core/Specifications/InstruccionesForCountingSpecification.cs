using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class InstruccionesForCountingSpecification:BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public InstruccionesForCountingSpecification(int id, InstruccionesSpecificationParams productoParams)

            : base(x => x.CEN_instruction.Creditor == id || x.CEN_instruction.Debtor == id)
        {

        }
    }
}
