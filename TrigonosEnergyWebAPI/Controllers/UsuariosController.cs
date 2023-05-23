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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using LogicaTrigonos.Logic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
        private readonly IRepositoryRolesUser _repoRolesUser;

        private readonly IMapper _mapper;
        private readonly RoleManager<Rol> _roleManager;
        private readonly IGenericRepository<REACT_TRGNS_UserProyects> _userProyects;
      
        public UsuariosController(IRepositoryRolesUser repo,UserManager<Usuarios> userManager, SignInManager<Usuarios> signInManager, ITokenService tokenService, IPasswordHasher<Usuarios> passwordHasher, IGenericSecurityRepository<Usuarios> seguridadRepository, IMapper mapper, RoleManager<Rol> roleManager, IGenericRepository<REACT_TRGNS_UserProyects> userProyects)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _seguridadRepository = seguridadRepository;
            _mapper = mapper;
            _roleManager = roleManager;
            _userProyects = userProyects;
            _repoRolesUser = repo;
        }
        /// <summary>
        /// Devuelve datos de usuario al logearse con su token
        /// </summary>M
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
                IdEmpresa = usuario.IdEmpresa,
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
        [Authorize]
        [HttpGet("pagination")]
        public async Task<ActionResult<Pagination<UsuariosDto>>> GetUsuarios([FromQuery] UsuarioSpecificationParams usuarioParams)
        {

            var currentRol = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if(currentRol == "Administrador")
            {
                var spec = new UsuarioSpecification(usuarioParams, currentRol);
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
            }else if (currentRol == "Admin Jefe")
            {
                var spec = new UsuarioSpecification(usuarioParams, currentRol);
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
            else
            {
                return NotFound(new CodeErrorResponse(404, "No tiene permisos, debe autenticarse"));
            }


            
        }
        [HttpGet("rolesUsers")]


        public  IReadOnlyList<AspNetUserRolesDto> GetUseRoles()
        {
        
            var participantes = _repoRolesUser.GetRolesUsuarios().ToList() ;  

            var maping = _mapper.Map<IReadOnlyList<AspNetUserRoles>, IReadOnlyList<AspNetUserRolesDto>>(participantes);
            return maping;
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
                IdEmpresa = usuario.IdEmpresa,
                Pais    = usuario.Pais,
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
            var proyectoUsuario = new REACT_TRGNS_UserProyects
            {
                idProyect = asignarProyectoDto.idProyect,
                idUser = asignarProyectoDto.idUser
            };

            if (!await _userProyects.SaveBD(proyectoUsuario))
            {
                return BadRequest(new CodeErrorResponse(500, "No existe el usuario y/o el proyecto"));
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
                IdEmpresa = usuario.IdEmpresa,
                Pais = usuario.Pais,
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
                return BadRequest(new CodeErrorResponse(400, "El email ingresado existe"));
            }
            var usuario = new Usuarios
            {

                Email = registrarDto.Email,
                UserName = registrarDto.Username,
                Nombre = registrarDto.Nombre,
                Apellido = registrarDto.Apellido,
                IdEmpresa = registrarDto.IdEmpresa,
                Pais    = registrarDto.Pais,
                Role = registrarDto.Rol
            };

            

            var resultado = await _userManager.CreateAsync(usuario, registrarDto.Password);

            var resultado1 = await _userManager.AddToRoleAsync(usuario, registrarDto.Rol);
            if (!resultado.Succeeded || !resultado1.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }
            else
            {
       

                List<REACT_TRGNS_UserProyects> listaNueva = registrarDto.ListIdProyects.Select(listitem =>
                new REACT_TRGNS_UserProyects
                {
                    idProyect = listitem,
                    idUser = usuario.Id
                }).ToList();
                if (!await _userProyects.SaveRangeBD(listaNueva))
                {
                    return BadRequest(new CodeErrorResponse(500, "No se logro eliminar el proyecto del usuario"));
                }
                else
                {

                    return new UsuariosDto

                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Token = _tokenService.CreateToken(usuario, registrarDto.Rol),
                        Email = usuario.Email,
                        Username = usuario.UserName,
                        IdEmpresa = usuario.IdEmpresa,
                        Pais = usuario.Pais,
                        Role = registrarDto.Rol
                    };
                }


             
            }


            
        }
        /// <summary>
        /// Actualizar datos de un usuario
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrarDto"></param>
        /// <returns></returns>
        [HttpPost("actualizar/{id}")]
        public async Task<ActionResult<UsuariosDto>> Actualizar(string id, ActualizarUserDto actualizarDto)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new CodeErrorResponse(404, "El usuario no existe"));

            }
            if (actualizarDto.Nombre != null)
            {
                usuario.Nombre = actualizarDto.Nombre;
            }
            if (actualizarDto.Apellido != null)
            {
                usuario.Apellido = actualizarDto.Apellido;
            }
            if (actualizarDto.Email != null)
            {
                usuario.Email = actualizarDto.Email;
            }
            if (actualizarDto.Username != null)
            {
                usuario.UserName = actualizarDto.Username;
            }
            if (actualizarDto.IdEmpresa != null)
            {
                usuario.IdEmpresa = actualizarDto.IdEmpresa;
            }
            if (actualizarDto.Pais != null)
            {
                usuario.Pais = actualizarDto.Pais;
            }
            if(actualizarDto.ListDeleteProyecto != null)
            {
                //List<ActualizarUserDto> deleteListUserProyects= actualizarDto.ListDeleteProyecto.ToList();

                try
                {
                    List<REACT_TRGNS_UserProyects> listaParaRemover = actualizarDto.ListDeleteProyecto.Select(listitem =>
                       new REACT_TRGNS_UserProyects
                       {
                           idProyect = listitem.idProyect,
                           idUser = listitem.idUser
                       }).ToList();
                    var entityToDelete = await _userProyects.GetAllAsync();

                    List<REACT_TRGNS_UserProyects> elementosCoincidentes = entityToDelete.Where(elementoLista1 =>
                                listaParaRemover.Any(elementoLista2 => elementoLista1.idProyect == elementoLista2.idProyect &&
                                                              elementoLista1.idUser == elementoLista2.idUser)
                            ).ToList();

                    if (!await _userProyects.RemoveRangeBD(elementosCoincidentes))
                    {
                        return BadRequest(new CodeErrorResponse(500, "No se logro eliminar el proyecto del usuario"));
                    }


                }
                catch (Exception)
                {
                    return BadRequest(new CodeErrorResponse(500, "No se logro eliminar el proyecto del usuario"));
                }
               
              

                //List<REACT_TRGNS_UserProyects> listaParaRemover = actualizarDto.ListDeleteProyecto.Select(listitem =>
                //new REACT_TRGNS_UserProyects
                //{
                //   idProyect = listitem.idProyect,
                //   idUser = listitem.idUser
                //}).ToList();
                //if (!await _userProyects.RemoveRangeBD(listaParaRemover))
                //{
                //    return BadRequest(new CodeErrorResponse(500, "No se logro eliminar el proyecto del usuario"));
                //}
               

            }
            if (actualizarDto.ListNewProyecto != null)
            {
                

                List<REACT_TRGNS_UserProyects> listaNueva = actualizarDto.ListNewProyecto.Select(listitem =>
                new REACT_TRGNS_UserProyects
                {
                    idProyect = listitem.idProyect,
                    idUser = listitem.idUser
                }).ToList();
                if (!await _userProyects.SaveRangeBD(listaNueva))
                {
                    return BadRequest(new CodeErrorResponse(500, "No se logro eliminar el proyecto del usuario"));
                }
              

            }


            if (actualizarDto.RolIdNuevo != null)
            {
                var RoleAnterior = await _roleManager.FindByIdAsync(actualizarDto.RolIdAnterior);
                var RoleNuevo = await _roleManager.FindByIdAsync(actualizarDto.RolIdNuevo);



                if (RoleAnterior == null)
                {
                    return NotFound(new CodeErrorResponse(404, "El Rol no existe"));

                }
                else
                {
                    var removeRole = await _userManager.RemoveFromRoleAsync(usuario, RoleAnterior.Name);
                    if (!removeRole.Succeeded)
                    {
                        return BadRequest(new CodeErrorResponse(400, "No se pudo remover el rol actual"));
                    }
                    else
                    {
                        if (RoleNuevo == null)
                        {
                            return NotFound(new CodeErrorResponse(404, "El Rol no existe"));
                        }
                        else
                        {
                            var addnewrol = await _userManager.AddToRoleAsync(usuario, RoleNuevo.Name);
                            if (!addnewrol.Succeeded)
                            {

                                return BadRequest(new CodeErrorResponse(400, "No se pudo agregar el nuevo rol"));

                            }
                            else
                            {
                                usuario.Role = RoleNuevo.Name;
                                var resultado1 = await _userManager.UpdateAsync(usuario);

                                if (!resultado1.Succeeded)
                                {
                                    return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el usuario"));
                                }
                                var roless = await _userManager.GetRolesAsync(usuario);
                                return new UsuariosDto
                                {
                                    Id = usuario.Id,
                                    Nombre = usuario.Nombre,
                                    Apellido = usuario.Apellido,
                                    Email = usuario.Email,
                                    Username = usuario.UserName,
                                    IdEmpresa = actualizarDto.IdEmpresa,
                                    Pais = actualizarDto.Pais,
                                    Token = _tokenService.CreateToken(usuario, roless[0]),
                                    Role = roless[0]
                                };
                            }
                        }

                    }
                }
            }
            else
            {
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
                    IdEmpresa = actualizarDto.IdEmpresa,
                    Pais = actualizarDto.Pais,
                    Token = _tokenService.CreateToken(usuario, roles[0]),
                    Role = roles[0]
                };
            }



            //Evaluar si lleva password o no yo creo que esto lo separare
            //if (!string.IsNullOrEmpty(registrarDto.Password))
            //{
            //    usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registrarDto.Password);
            //}


           





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
            if (role == null)
            {

                return NotFound(new CodeErrorResponse(404, "El role no existe"));
            }

            if (usuario == null)
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
                    if (resultado.Errors.Where(x => x.Code == "UserAlreadyRole").Any())
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

        [HttpPost("ValidarEmail")]
        //public async Task<ActionResult<UsuariosDto>> ExisteUsuario([FromQuery] string email)
        public async Task<ActionResult<UsuariosDto>> ValidarEmail([FromQuery] string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);
            //var roles = await _userManager.GetRolesAsync(usuario);
            if (usuario == null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }
            return new UsuariosDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.UserName,
                IdEmpresa = usuario.IdEmpresa,
                Pais = usuario.Pais,
                Token = _tokenService.CreateToken(usuario),
                Role = ""
            };

        }
        /// <summary>
        /// Recuperar contraseña
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ActualizarContrasena")]
        public async Task<ActionResult<UsuariosDto>> RecuperarContrasena(ActualizarContrasenaDto actualizarContrasenaDto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var usuario = await _userManager.FindByEmailAsync(email);
            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, actualizarContrasenaDto.Password);
            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el usuario"));
            }
            return new UsuariosDto
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
            };




        }
    }
}
