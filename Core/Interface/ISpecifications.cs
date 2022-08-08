using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface ISpecifications<T>
    {
        //Condicion Logica de la consulta
        Expression<Func<T, bool>> Criteria { get; }
        //Representa las relaciones que podremos implementar sobre una entidad
        List<Expression<Func<T, object>>> Includes { get; }

        
    }
}
