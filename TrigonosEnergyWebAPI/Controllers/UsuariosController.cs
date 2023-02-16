using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;

        public UsuariosController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UsuariosDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);
            if(usuario == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }
            var result = await _signInManager.CheckPasswordSignInAsync(usuario,loginDto.Password,false);
            if (!result.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }
            return new UsuariosDto
            {
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = "Este es el token del usuario",
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };

        }


    }
}
