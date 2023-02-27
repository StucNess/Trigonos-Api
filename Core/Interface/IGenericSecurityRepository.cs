using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IGenericSecurityRepository<T> where T : IdentityUser
    {
        //Debe obtener el id con la especificacion
        Task<T> GetByClienteIDAsync(ISpecifications<T> spec);
        Task<T> GetByClienteIDAsync(string id);
        //Debe obtener todo con la especificacion
        Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<bool> UpdateeAsync(T BD);
        //Task<T> GetIDAsync(int id);
        Task<IReadOnlyList<T>> GetAllInstrucctionByIdAsync(ISpecifications<T> spec);
        Task<int> CountAsync(ISpecifications<T> spec);
        Task<bool> SaveBD(T BD);
    }
}
