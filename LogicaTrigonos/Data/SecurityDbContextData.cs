using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data
{
    public class SecurityDbContextData
    {
        public static async Task SeedUserAsync(UserManager<Usuarios> userManager)
        {
            if (!userManager.Users.Any()) {
                var usuario = new Usuarios
                {
                    Nombre = "javier",
                    Apellido = "onate",
                    UserName = "javier1233",
                    Email = "ejoocontactos@gmail.com"
                };

                await userManager.CreateAsync(usuario,"Colocolo1$");
                

            }
        }
    }
}
