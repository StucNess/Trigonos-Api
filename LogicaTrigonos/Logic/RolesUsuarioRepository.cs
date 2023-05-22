using Core.Entities;
using Core.Interface;
using LogicaTrigonos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Logic
{
    public class RolesUsuarioRepository : IRepositoryRolesUser
    {
        private readonly TrigonosDBContext _bd;
        public RolesUsuarioRepository(TrigonosDBContext bd)
        {
            _bd = bd;
        }
        public ICollection<AspNetUserRoles> GetRolesUsuarios()
        {
            return _bd.AspNetUserRoles.OrderBy(c => c.RoleId).ToList();
        }
       
    }
}
