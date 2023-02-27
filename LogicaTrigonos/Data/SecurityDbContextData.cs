using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaTrigonos.Data
{
    public class SecurityDbContextData
    {
        public static async Task SeedUserAsync(UserManager<Usuarios> userManager,RoleManager<IdentityRole> roleManager)
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
            if (!roleManager.Roles.Any())
            {
                var role = new IdentityRole
                {
                    Name = "admin"
                };
                var role1 = new IdentityRole
                {
                    Name = "cliente"
                };
                var role2 = new IdentityRole
                {
                    Name = "trgns"
                };
                var role3 = new IdentityRole
                {
                    Name = "no"
                };
                await roleManager.CreateAsync(role);
                await roleManager.CreateAsync(role1);
                await roleManager.CreateAsync(role2);
                await roleManager.CreateAsync(role3);
            }
            
        }
    }
}
