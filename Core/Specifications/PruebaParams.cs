﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class PruebaParams : BaseSpecification<TRGNS_Datos_Facturacion>
    {
        public PruebaParams(int id/*, InstruccionesSpecificationParams productoParams*/)
        

           : base(x => (x.id_instructions == id ))
           

           { } 
    }
}
