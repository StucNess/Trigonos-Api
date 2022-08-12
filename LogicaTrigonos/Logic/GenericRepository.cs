﻿using Core.Entities;
using Core.EntitiesPatch;
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

        //public async Task<IReadOnlyList<T>> GetAllAsyncc()
        //{

        //}
        //public async Task<T> GetIDAsync(int id)
        //{
        //    return await _context.Set<T>().FirstOrDefault(p=> p.ID == id);
        //}
        //public bool Updatee(T BD)
        //{

        //}

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

        //public bool UpdateeAsync(Patch_TRGNS_Datos_Facturacion pro)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByClienteIDAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(p => p.ID == id);
        }
    }
}
