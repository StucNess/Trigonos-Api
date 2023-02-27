using Core.Interface;
using LogicaTrigonos.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Logic
{
    public class GenericSecurityRepository<T> : IGenericSecurityRepository<T> where T : IdentityUser
    {
        private readonly SecurityDbContext _context;

        public GenericSecurityRepository(SecurityDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllInstrucctionByIdAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        //Desde el objeto SPEC vienen las relaciones 
        public async Task<T> GetByClienteIDAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SecuritySpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
        public async Task<int> CountAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }


        public async Task<bool> UpdateeAsync(T BD)
        {
            _context.Set<T>().Update(BD);
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }


        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByClienteIDAsync(string id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SaveBD(T BD)
        {
            _context.Set<T>().Add(BD);
            _context.SaveChanges();
            return true;
        }

        //public Task<T> GetByClienteIDAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

