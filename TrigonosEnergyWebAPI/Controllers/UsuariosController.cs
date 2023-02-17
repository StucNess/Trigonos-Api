using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;
        private readonly ITokenService _tokenService;

        public UsuariosController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
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
                Token = _tokenService.CreateToken(usuario),
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };

        }
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuariosDto>> Registrar(RegistrarDto registrarDto)
        {
            var usuario = new Usuarios
            {
                Email = registrarDto.Email,
                UserName = registrarDto.Username,
                Nombre = registrarDto.Nombre,
                Apellido = registrarDto.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario,registrarDto.Password);
            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }
            return new UsuariosDto
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = _tokenService.CreateToken(usuario),
                Email = usuario.Email,
                Username = usuario.UserName 
            };

        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UsuariosDto>> GetUsuario()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var usuario = await _userManager.FindByEmailAsync(email);
            return new UsuariosDto
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario)
            };
        }
        [Authorize]
        [HttpGet("emailvalid")]
        public async Task<ActionResult<bool>> ValidarEmail([FromQuery]string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            if (usuario == null) return false;
            return true;
        }

    }
}
