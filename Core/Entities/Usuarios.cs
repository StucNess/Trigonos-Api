using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Usuarios:IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? IdEmpresa { get; set; }
        public string? Pais { get; set; }
        public string? Role { get; set; }

        //public Direccion Direccion { get; set; }

    }
}
