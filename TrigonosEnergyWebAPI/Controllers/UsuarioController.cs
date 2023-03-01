//using AutoMapper;
//using Core.Entities;
//using Core.Interface;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using TrigonosEnergy.Controllers;
//using TrigonosEnergyWebAPI.DTO;
//using TrigonosEnergyWebAPI.Errors;

//namespace TrigonosEnergyWebAPI.Controllers
//{
//    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    [ApiExplorerSettings(GroupName = "APIUsuarios")]
//    public class UsuarioController : BaseApiController
//    {
//        private readonly IRepositoryUsuario _repo;
//        private readonly IMapper _mapper;
//        private readonly IConfiguration _config;

//        public UsuarioController(IRepositoryUsuario repo, IMapper mapper, IConfiguration config)
//        {
//            _repo = repo;
//            _mapper = mapper;
//            _config = config;
//        }
//        /// <summary>
//        /// Obtener a los usuarios
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        public IActionResult GetUsuarios()
//        {
//            var listaUsuarios = _repo.GetUsuarios();
//            var listaUsuariosDto = new List<UsuarioDto>();
//            foreach (var lista in listaUsuarios)
//            {
//                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
//            }
//            return Ok(listaUsuariosDto);

//        }
//        /// <summary>
//        /// Obtener a un usuario en especifico
//        /// </summary>
//        /// <param name="Id"></param>
//        /// <returns></returns>
        
//        [HttpGet("{Id:int}", Name = "GetUsuario")]
//        public IActionResult GetUsuario(int Id)
//        {
//            var itemUsuario = _repo.GetUsuario(Id);
//            if (itemUsuario == null)
//            {
//                return NotFound(new CodeErrorResponse(404));
//            }
//            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
//            return Ok(itemUsuarioDto);
//        }
//        /// <summary>
//        /// Registrar a un usuario
//        /// </summary>
//        /// <param name="usuarioAuthDto"></param>
//        /// <returns></returns>
//        [HttpPost("Registro")]
//        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
//        {
//            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();
//            if (_repo.ExisteUsuario(usuarioAuthDto.Usuario))
//            {
//                return BadRequest("El usuario ya existe");
//            }
//            var usuarioACrear = new Usuario
//            {
//                UsuarioA = usuarioAuthDto.Usuario
//            };
//            var usuarioCreado = _repo.Registro(usuarioACrear,usuarioAuthDto.Password);
//            return Ok(usuarioCreado);
//        }
//        /// <summary>
//        /// Logearse como usuario
//        /// </summary>
//        /// <param name="usuarioAuthLoginDto"></param>
//        /// <returns></returns>
//        [AllowAnonymous]
//        [HttpPost("Login")]
//        public IActionResult Login(UsuarioAuthLoginDto usuarioAuthLoginDto)
//        {
//            var usuario = _repo.Login(usuarioAuthLoginDto.Usuario,usuarioAuthLoginDto.Password);
//            if (usuario == null)
//            {
//                return Unauthorized();
//            }
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier,usuario.ID.ToString()),
//                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioA.ToString())
//        };
//            // Generacion de token
//            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
//            var credenciales = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {

//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.Now.AddDays(1),
//                SigningCredentials = credenciales,
//            };
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var token = tokenHandler.CreateToken(tokenDescriptor);

//            return Ok(new
//            {
//                token = tokenHandler.WriteToken(token)
//            });
//        }
            
//    }
//}
