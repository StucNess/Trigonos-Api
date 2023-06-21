using Core.Entities;
using Core.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data
{
    public class SpecificationEvaluator<T> where T : TRGNS_base
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            if(spec.Criteria !=null)
            {
                inputQuery = inputQuery.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                inputQuery = inputQuery.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
            }

            

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));
            if (spec.DistinctBy != null)
            {
                inputQuery = inputQuery.GroupBy(spec.DistinctBy).Select(query => query.First());
            }
            if (spec.IsPagingEnabled)
            {
                inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
            }
            return inputQuery;
        }
    }
}
