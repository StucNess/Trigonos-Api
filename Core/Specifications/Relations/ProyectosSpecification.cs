using Core.Entities;
using Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications.Relations
{
    public class ProyectosSpecification :BaseSpecification<REACT_TRGNS_PROYECTOS>
    {
        public ProyectosSpecification(ProyectosSpecificationParams proyectoparams)
        {
            ApplyPaging(proyectoparams.PageSize * (proyectoparams.PageIndex - 1), proyectoparams.PageSize);
        }
       
    }
}
