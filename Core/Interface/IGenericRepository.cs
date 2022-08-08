using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGenericRepository<T> where T : TRGNS_base
    {
        //Debe obtener el id con la especificacion
        Task<T> GetByClienteIDAsync(ISpecifications<T> spec);
        //Debe obtener todo con la especificacion
        Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec);
        
        
    }
}
