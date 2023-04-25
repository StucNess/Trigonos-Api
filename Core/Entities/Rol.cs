using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Rol: IdentityRole
    {
        public string? Descripcion { get; set; }
        public int? Bhabilitado { get; set; }
    }
}
