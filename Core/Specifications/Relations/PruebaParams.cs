using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class PruebaParams : BaseSpecification<REACT_CEN_instructions_Def>
    {
        public PruebaParams(int id/*, InstruccionesSpecificationParams productoParams*/)


           : base(x => x.ID == id)

        {
        }
    }
}
