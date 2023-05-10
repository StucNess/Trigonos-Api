﻿using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Relations;
using LogicaTrigonos.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.WsTrust;
using TrigonosEnergy.Controllers;
using TrigonosEnergy.DTO;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIRol")]
    public class RolController : BaseApiController
    {
        private readonly IGenericRepository<REACT_TRGNS_PaginasWeb> _paginasWebRepository;
        private readonly IGenericRepository<REACT_TRGNS_RolPaginas> _rolPaginasRepository;
        private readonly IGenericRepositoryRole<Rol> _rolRepository;
        private readonly RoleManager<Rol> _rolManager;
        private readonly IMapper _mapper;
        public RolController(RoleManager<Rol> roleManager, IGenericRepositoryRole<Rol> rolRepository, IGenericRepository<REACT_TRGNS_PaginasWeb> paginasWebRepository, IGenericRepository<REACT_TRGNS_RolPaginas> rolPaginasRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
            _rolManager = roleManager;
            _paginasWebRepository= paginasWebRepository;
            _rolPaginasRepository = rolPaginasRepository;
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
        //CRUD PAGINA_WEB
        [HttpPost("AgregarPaginaWeb")]
        public async Task<IActionResult> AgregarPagina(AgregarPaginaWeb agregarPagina)
        {

            var paginaweb = new REACT_TRGNS_PaginasWeb
            {
                Nombre = agregarPagina.Nombre,
                Descripcion = agregarPagina.Descripcion,
                Bhabilitado = agregarPagina.Bhabilitado
            };

   

            if (!await _paginasWebRepository.SaveBD(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha agregado la empresa"));
            }
            return Ok();
        }
        [HttpGet("ListarPaginaWeb")]

        public async Task<ActionResult<IReadOnlyList<PaginasWebDto>>> GetPaginasWebs()
        {
            var datos = await _paginasWebRepository.GetAllAsync();
            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_PaginasWeb>, IReadOnlyList<PaginasWebDto>>(datos);
            return Ok(maping);
        }

        [HttpPost("actualizarPaginaWeb/{id}")]
        public async Task<IActionResult> ActualizarPaginaWeb(int id, AgregarPaginaWeb registrarDto)
        {
            var paginaweb = await _paginasWebRepository.GetByClienteIDAsync(id);
            if (paginaweb == null)
            {
                return NotFound(new CodeErrorResponse(404, "La pagina web no existe"));

            }
            paginaweb.Nombre = registrarDto.Nombre;
            paginaweb.Descripcion = registrarDto.Descripcion;
            //paginaweb.Bhabilitado = registrarDto.Bhabilitado; esta el desactivar y activar para esto :V



            if (!await _paginasWebRepository.UpdateeAsync(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo actualizar los datos de la pagina web"));
            }
          
            return Ok();
        }
        [HttpPost("desactivarPaginaWeb/{id}")]
        public async Task<IActionResult> DesactivarPaginaWeb(int id)
        {
            var paginaweb = await _paginasWebRepository.GetByClienteIDAsync(id);
            if (paginaweb == null)
            {
                return NotFound(new CodeErrorResponse(404, "La pagina web no existe"));

            }
            else if (paginaweb.Bhabilitado==0)
            {
                return NotFound(new CodeErrorResponse(404, "ya se encuentra desactivada!"));
            }

            paginaweb.Bhabilitado =0;



            if (!await _paginasWebRepository.UpdateeAsync(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo desactivar la pagina web"));
            }

            return Ok();
        }
        [HttpPost("activarPaginaWeb/{id}")]
        public async Task<IActionResult> activarPaginaWeb(int id)
        {
            var paginaweb = await _paginasWebRepository.GetByClienteIDAsync(id);
            if (paginaweb == null)
            {
                return NotFound(new CodeErrorResponse(404, "La pagina web no existe"));

            }
            else if (paginaweb.Bhabilitado == 1)
            {
                return NotFound(new CodeErrorResponse(404, "ya se encuentra activada!"));
            }

            paginaweb.Bhabilitado = 1;



            if (!await _paginasWebRepository.UpdateeAsync(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo activar la pagina web"));
            }

            return Ok();
        }
        [HttpPost("eliminarPaginaWeb/{id}")]
        public async Task<IActionResult> eliminarPaginaWeb(int id)
        {
            var paginaweb = await _paginasWebRepository.GetByClienteIDAsync(id);
            if (paginaweb == null)
            {
                return NotFound(new CodeErrorResponse(404, "La pagina web no existe"));

            }

            if (!await _paginasWebRepository.RemoveBD(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo activar la pagina web"));
            }

            return Ok();
        }



        //CRUD rol_pagina
        [HttpPost("AsignarRolPagina")]
        public async Task<IActionResult> AsignarRolPagina(AsignarRolPaginaDto asignarPagina)
        {


            var spec = new RolPaginasRelation();
            var rolPaginas = await _rolPaginasRepository.GetAllAsync(spec);
            var existingItem = rolPaginas.FirstOrDefault(i => i.Idpagina == asignarPagina.Idpagina && i.Idrol == asignarPagina.Idrol);

           
            var rolpagina = new REACT_TRGNS_RolPaginas
            {
                Idrol = asignarPagina.Idrol,
                Idpagina = asignarPagina.Idpagina,
                Bhabilitado = asignarPagina.Bhabilitado
            };
            if (existingItem != null)
            {
                return NotFound(new CodeErrorResponse(404, "Error ya existe un idrol y un idpagina en la tabla"));
            }

            if ( !await _rolPaginasRepository.SaveBD(rolpagina))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha logrado asignar el rol a la pagina"));
            }
            return Ok();
        }
        [HttpPost("quitarRolPagina/{Idrol}/{Idpagina}")]//Elimina defenitivamente de la tabla de rompimiento pero la pagina obvio sigue existiendo
        public async Task<IActionResult> quitarRolPagina(string Idrol, int Idpagina)
        {


            var spec = new RolPaginasRelation();
            var rolPaginas = await _rolPaginasRepository.GetAllAsync(spec);
            var existingItem = rolPaginas.FirstOrDefault(i => i.Idpagina == Idpagina && i.Idrol == Idrol);

            if (existingItem == null)
            {
                return NotFound(new CodeErrorResponse(404, "Error no existe un idrol y un idpagina en la tabla"));
            }

            if (!await _rolPaginasRepository.RemoveBD(existingItem))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha logrado eliminar el rol a la pagina"));
            }
            return Ok();
        }
        [HttpGet("listarRolPagina")] //lista la tabla de romperompe

        public async Task<ActionResult<IReadOnlyList<RolPaginaWebDto>>> GetRolesPaginas()
        {
            var spec = new RolPaginasRelation();
            var participantes = await _rolPaginasRepository.GetAllAsync(spec);
            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_RolPaginas>, IReadOnlyList<RolPaginaWebDto>>(participantes);
            return Ok(maping);
        }
        [HttpPost("desactivarRolPagina/{Idrol}/{Idpagina}")]
        public async Task<IActionResult> DesactivarRolPagina(string Idrol, int Idpagina)
        {
            var spec = new RolPaginasRelation();
            var rolPaginas = await _rolPaginasRepository.GetAllAsync(spec);
            var existingItem = rolPaginas.FirstOrDefault(i => i.Idpagina == Idpagina && i.Idrol == Idrol);
            if (existingItem == null)
            {
                return NotFound(new CodeErrorResponse(404, "No existe ese rolpage no existe"));

            }
            else if (existingItem.Bhabilitado == 0)
            {
                return NotFound(new CodeErrorResponse(404, "ya se encuentra desactivado el rol para la pagina!"));
            }
            var paginaweb = await _rolPaginasRepository.GetByClienteIDAsync(existingItem.ID);
            paginaweb.Bhabilitado = 0;

            if (!await _rolPaginasRepository.UpdateeAsync(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo desactivar el rol de la pagina"));
            }

            return Ok();
        }
        [HttpPost("activarRolPagina/{Idrol}/{Idpagina}")]
        public async Task<IActionResult> ActivarRolPagina(string Idrol, int Idpagina)
        {
            var spec = new RolPaginasRelation();
            var rolPaginas = await _rolPaginasRepository.GetAllAsync(spec);
            var existingItem = rolPaginas.FirstOrDefault(i => i.Idpagina == Idpagina && i.Idrol == Idrol);
            if (existingItem == null)
            {
                return NotFound(new CodeErrorResponse(404, "No existe ese rolpage no existe"));

            }
            else if (existingItem.Bhabilitado == 1)
            {
                return NotFound(new CodeErrorResponse(404, "ya se encuentra activado el rol para la pagina!"));
            }
            var paginaweb = await _rolPaginasRepository.GetByClienteIDAsync(existingItem.ID);
            paginaweb.Bhabilitado = 1;

            if (!await _rolPaginasRepository.UpdateeAsync(paginaweb))
            {
                return BadRequest(new CodeErrorResponse(400, "No se pudo activado el rol de la pagina"));
            }

            return Ok();
        }

    }
}
