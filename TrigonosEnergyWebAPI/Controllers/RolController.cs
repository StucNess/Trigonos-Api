using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIRol")]
    public class RolController : BaseApiController
    {

        private readonly IGenericRepositoryRole<Rol> _rolRepository;
        private readonly RoleManager<Rol> _rolManager;
        private readonly IMapper _mapper;
        public RolController(RoleManager<Rol> roleManager, IGenericRepositoryRole<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
            _rolManager = roleManager;
        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<RolDto>>> GetRoles()
        {
            var datos =  await _rolManager.Roles.ToListAsync();
            var maping = _mapper.Map<IReadOnlyList<Rol>, IReadOnlyList<RolDto>>(datos);
            return Ok(maping);
        }
        [HttpPost("Prueba")]
        public int Prueba()
        {
            var usuarioEmail = 1 + 1;

            return usuarioEmail;
        }
        [HttpPost("Agregar")]
        public async Task<ActionResult<RolDto>> AgregarRol(AgregarRolDto agregarRolDto)
        {
            var rol = new Rol
            {

                Name = agregarRolDto.Name,
                Descripcion = agregarRolDto.Descripcion,
                Bhabilitado = 1, //agregarRolDto.Bhabilitado

            };
            var resultado = await _rolManager.CreateAsync(rol);
            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha agregado el rol"));
            }
            return Ok();
        }


        [HttpPatch("Actualizar")]

        public async Task<ActionResult<RolDto>> Actualizar(string id, AgregarRolDto parametros)
        {


            var rol = await _rolManager.FindByIdAsync(id);
            if (rol == null)
            {
                return NotFound(new CodeErrorResponse(404, "El Rol no existe"));

            }
            rol.Name = parametros.Name;
            rol.Descripcion = parametros.Descripcion;
            rol.Bhabilitado = parametros.Bhabilitado;
            var resultado = await _rolManager.UpdateAsync(rol);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar el Rol"));
            }
            return Ok();
        }
    }
}
