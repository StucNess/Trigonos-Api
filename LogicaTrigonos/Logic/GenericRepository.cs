using Core.Entities;
using Core.Interface;
using LogicaTrigonos.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Logic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : TRGNS_base
    {
        private readonly TrigonosDBContext _context;

        public GenericRepository(TrigonosDBContext context)
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
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
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

        public async Task<T> GetByClienteIDAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(p => p.ID == id);
        }

        public async Task<bool> SaveBD(T BD)
        {
            _context.Set<T>().Add(BD);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveBD(T BD)
        {
            _context.Set<T>().Remove(BD);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> RemoveRangeBD(IEnumerable<T> BD)
        {
            _context.Set<T>().RemoveRange(BD);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> SaveRangeBD(IEnumerable<T> BD)
        {
            _context.Set<T>().AddRange(BD);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> UpdateRangeBD(IEnumerable<T> BD)
        {
            _context.Set<T>().UpdateRange(BD);
            _context.SaveChanges();
            return true;
        }
    }
}
