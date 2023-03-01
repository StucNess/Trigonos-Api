using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using System.Security.Claims;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIUsuarios")]
    public class UsuariosController : BaseApiController
    {
        private readonly UserManager<Usuarios> _userManager;
        private readonly SignInManager<Usuarios> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<Usuarios> _passwordHasher;
        private readonly IGenericSecurityRepository<Usuarios> _seguridadRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IGenericRepository<TRGNS_UserProyects> _userProyects;

        
        public UsuariosController(UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, ITokenService tokenService, IPasswordHasher<Usuarios> passwordHasher, IGenericSecurityRepository<Usuarios> seguridadRepository, IMapper mapper, RoleManager<IdentityRole> roleManager, IGenericRepository<TRGNS_UserProyects> userProyects)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _seguridadRepository = seguridadRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _userProyects = userProyects;
        }
        /// <summary>
        /// Devuelve datos de usuario al logearse con su token
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UsuariosDto>> GetUsuario()
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var usuario = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(usuario);
            return new UsuariosDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario, roles[0]),
                Role = roles[0]
            };
        }
        //[HttpGet("emailvalid")]
        //public async Task<ActionResult<bool>> ValidarEmail([FromQuery] string email)
        //{
        //    var usuario = await _userManager.FindByEmailAsync(email);
        //    if (usuario == null) return false;
        //    return true;
        //}
        /// <summary>
        /// Devulve a todos los usuarios
        /// </summary>
        /// <param name="usuarioParams"></param>
        /// <returns></returns>
        [HttpGet("pagination")]
        public async Task<ActionResult<Pagination<UsuariosDto>>> GetUsuarios([FromQuery] UsuarioSpecificationParams usuarioParams)
        {
            var spec = new UsuarioSpecification(usuarioParams);
            var usuarios = await _seguridadRepository.GetAllAsync(spec);
            var specCount = new UsuarioForCountingSpecification(usuarioParams);
            var totalUsuarios = await _seguridadRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalUsuarios) / Convert.ToDecimal(usuarioParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<Usuarios>, IReadOnlyList<UsuariosDto>>(usuarios);
            return Ok(
                new Pagination<UsuariosDto>
                {

                    count = totalUsuarios,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = usuarioParams.PageIndex,
                    PageSize = usuarioParams.PageSize,
                }
                );
        }
        /// <summary>
        /// Obtener datos del usuario con su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("account/{id}")]
        public async Task<ActionResult<UsuariosDto>> GetUsuarioBy(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "el usuario no existe"));
            }
            var roles = await _userManager.GetRolesAsync(usuario);
            return new UsuariosDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Role = roles[0]
            };
        }
        /// <summary>
        /// Asigna a usuario permiso para administrar un proyecto
        /// </summary>
        /// <param name="asignarProyectoDto"></param>
        /// <returns></returns>
        [HttpPost("AsignarProyecto")]
        public async Task<IActionResult> AsignarProyecto(AsignarProyectoDto asignarProyectoDto)
        {
            var proyectoUsuario = new TRGNS_UserProyects
            {
                idProyect = asignarProyectoDto.idProyect,
                idUser = asignarProyectoDto.idUser
            };

            if (!await _userProyects.SaveBD(proyectoUsuario))
            {
                return BadRequest(new CodeErrorResponse(500,"No existe el usuario y/o el proyecto"));
            }
            return Ok();
        }
        /// <summary>
        /// Retorna datos del usuario al ingresar sus credenciales
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<UsuariosDto>> Login(LoginDto loginDto)
        {
            var usuario = await _userManager.FindByEmailAsync(loginDto.Email);

            if (usuario == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }
            var result = await _signInManager.CheckPasswordSignInAsync(usuario, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            return new UsuariosDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario, roles[0]),
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Role = roles[0]

            };

        }
        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        /// <param name="registrarDto"></param>
        /// <returns></returns>
        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuariosDto>> Registrar(RegistrarDto registrarDto)
        {   
            var usuarioEmail = await _userManager.FindByEmailAsync(registrarDto.Email);
            if (usuarioEmail != null)
            {
                return BadRequest(new CodeErrorResponse(400,"El email ingresado existe"));
            }
            var usuario = new Usuarios
            {

                Email = registrarDto.Email,
                UserName = registrarDto.Username,
                Nombre = registrarDto.Nombre,
                Apellido = registrarDto.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, registrarDto.Password);



            var resultado1 = await _userManager.AddToRoleAsync(usuario, registrarDto.Rol);
            if (!resultado.Succeeded || !resultado1.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }
            return new UsuariosDto

            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Token = _tokenService.CreateToken(usuario, registrarDto.Rol),
                Email = usuario.Email,
                Username = usuario.UserName,
                Role = registrarDto.Rol
            };
        }
        /// <summary>
        /// Actualizar datos de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrarDto"></param>
        /// <returns></returns>
        [HttpPut("actualizar/{id}")]
        public async Task<ActionResult<UsuariosDto>> Actualizar(string id, RegistrarDto registrarDto)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));

            }
            usuario.Nombre = registrarDto.Nombre;
            usuario.Apellido = registrarDto.Apellido;

            if (!string.IsNullOrEmpty(registrarDto.Password))
            {
                usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registrarDto.Password);
            }

            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el usuario"));
            }
            var roles = await _userManager.GetRolesAsync(usuario);
            return new UsuariosDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario, roles[0]),
                Role = roles[0]
            };
        }
        /// <summary>
        /// Actualiza el rol de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleParam"></param>
        /// <returns></returns>
        [HttpPut("role/{id}")]
        public async Task<ActionResult<UsuariosDto>> UpdateRole(string id, RoleDto roleParam)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            var role = _roleManager.FindByNameAsync(roleParam.Nombre);
            if(role == null)
            {

                return NotFound(new CodeErrorResponse(404, "El role no existe"));
            }
            
            if(usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));
            }
            var usuarioDto = _mapper.Map<Usuarios, UsuariosDto>(usuario);
            if (roleParam.Status)
            {
                var resultado = await _userManager.AddToRoleAsync(usuario, roleParam.Nombre);
                if (resultado.Succeeded)
                {
                    usuarioDto.Role = roleParam.Nombre;
                }
                if (resultado.Errors.Any())
                {
                    if(resultado.Errors.Where(x=> x.Code == "UserAlreadyRole").Any())
                    {
                        usuarioDto.Role = roleParam.Nombre;
                    }
                   
                }
                //else
                //{
                //    var resultado1 = await _userManager.RemoveFromRoleAsync(usuario, roleParam.Nombre);
                //    if (resultado1.Succeeded)
                //    {
                //        usuarioDto.Role = "NO";
                //    }

                //}
            }
            else
            {
               var resultado = await _userManager.RemoveFromRoleAsync(usuario, roleParam.Nombre);
                if (resultado.Succeeded)
                {
                    usuarioDto.Role = "NO";
                }
                
            }
            usuarioDto.Token = _tokenService.CreateToken(usuario, roleParam.Nombre);
            return usuarioDto;
        }
    }
}
