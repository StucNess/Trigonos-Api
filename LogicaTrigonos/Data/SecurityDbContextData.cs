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
                    //Direccion = new Direccion
                    //{
                    //    Calle = "Avenida Fermin Vergara 218",
                    //    Ciudad = "Colina",
                    //    CodigoPostal = "93030",
                    //    Departamento = "No"
                    //}
                };

                await userManager.CreateAsync(usuario,"Colocolo1$");
                

            }
        }
    }
}
