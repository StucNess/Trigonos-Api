using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{

    public class UsuarioController : BaseApiController
    {
        private readonly IRepositoryUsuario _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsuarioController(IRepositoryUsuario repo, IMapper mapper, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _config = config;
        }
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _repo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }
        [HttpGet("{Id:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int Id)
        {
            var itemUsuario = _repo.GetUsuario(Id);
            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }
        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();
            if (_repo.ExisteUsuario(usuarioAuthDto.Usuario))
            {
                return BadRequest("El usuario ya existe");
            }
            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioAuthDto.Usuario
            };
            var usuarioCreado = _repo.Registro(usuarioACrear,usuarioAuthDto.Password);
            return Ok(usuarioCreado);
        }

        [HttpPost("Login")]
        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLoginDto)
        {
            var usuario = _repo.Login(usuarioAuthLoginDto.Usuario,usuarioAuthLoginDto.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,usuario.ID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioA.ToString())
        };
            // Generacion de token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            //
            var credenciales = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(200),
                SigningCredentials = credenciales,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
            });
        }
            
    }
}
