﻿using Core.Entities;

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
        Task<T> GetByClienteIDAsync(int id);
        //Debe obtener todo con la especificacion
        Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<bool> UpdateeAsync(T BD);
        //Task<T> GetIDAsync(int id);
        Task<IReadOnlyList<T>> GetAllInstrucctionByIdAsync(ISpecifications<T> spec);
        Task<int> CountAsync(ISpecifications<T> spec);
        Task<bool> SaveBD(T BD);
        Task<bool> RemoveBD(T BD);
        Task<bool> RemoveRangeBD(IEnumerable<T> BD);
        Task<bool> SaveRangeBD(IEnumerable<T> BD);
        Task<bool> UpdateRangeBD(IEnumerable<T> BD);


    }
}
