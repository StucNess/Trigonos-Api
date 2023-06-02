using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using TrigonosEnergy.Controllers;
using TrigonosEnergy.DTO;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIFacturacionCl")]
    public class FacturacionClController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<REACT_TRGNS_FACTCLDATA> _factClRepository;
        private readonly IGenericRepository<REACT_CEN_Participants> _participantesRepository;

        public FacturacionClController(IMapper mapper, IGenericRepository<REACT_TRGNS_FACTCLDATA> factClRepository)
        {
            _mapper = mapper;
            _factClRepository = factClRepository;

        }
        string EncodeToBase64(string input)
        {
            var output = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            return output;
        }
        string DecodeToString(string input)
        {
            var output = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input));
            return output;
        }
        /// <summary>
        /// Obtener la facturacion cl con paginacion
        /// </summary>
        [HttpGet("PaginationDecode")]
        [ProducesResponseType(200, Type = typeof(Pagination<FacturacionClDto>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<FacturacionClDto>>> GetPaginationFacturacionCl(int PageIndex = 0, int PageSize = 1)
        {

            var encodeList = await _factClRepository.GetAllAsync();
            List<REACT_TRGNS_FACTCLDATA> listDecode = encodeList.Select(listitem =>
                       new REACT_TRGNS_FACTCLDATA
                       {
                           ID = listitem.ID,
                           IdParticipante = listitem.IdParticipante,
                           Usuario64 = listitem.Usuario64 != null ? DecodeToString(listitem.Usuario64) : null,
                           RUT64 = listitem.RUT64 != null ? DecodeToString(listitem.RUT64) : null,
                           Clave64 = listitem.Clave64 != null ? DecodeToString(listitem.Clave64) : null,
                           Puerto64 = listitem.Puerto64 != null ? DecodeToString(listitem.Puerto64) : null,
                           IncluyeLink64 = listitem.IncluyeLink64 != null ? DecodeToString(listitem.IncluyeLink64) : null,
                           UsuarioTest = listitem.UsuarioTest != null ? DecodeToString(listitem.UsuarioTest) : null,
                           ClaveTest = listitem.ClaveTest != null ? DecodeToString(listitem.ClaveTest) : null,
                           RutTest = listitem.RutTest != null ? DecodeToString(listitem.RutTest) : null,
                           Phabilitado = listitem.Phabilitado,
                       }).ToList();

            var total= listDecode.Count();
            var rounded = Math.Ceiling(Convert.ToDecimal(total/PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_FACTCLDATA>, IReadOnlyList<FacturacionClDto>>(listDecode);
            return Ok(
                new Pagination<FacturacionClDto>
                {
                    count = total,
                    Data = data,
                    PageCount = totalPages + 1,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                }
                );


        }

        [HttpGet("PaginationEncode")]
        [ProducesResponseType(200, Type = typeof(Pagination<FacturacionClDto>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<FacturacionClDto>>> GetPaginationEncodeFacturacionCl(int PageIndex = 0, int PageSize = 1)
        {


            var encodeList = await _factClRepository.GetAllAsync();
            var total = encodeList.Count();
            var rounded = Math.Ceiling(Convert.ToDecimal(total / PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_FACTCLDATA>, IReadOnlyList<FacturacionClDto>>(encodeList);
            return Ok(
                new Pagination<FacturacionClDto>
                {
                    count = total,
                    Data = data,
                    PageCount = totalPages + 1,
                    PageIndex = PageIndex,
                    PageSize = PageSize,
                }
                );
        }
        /// <summary>
        /// Obtener la facturacion cl sin paginacion y sin decodificacion
        /// </summary>
        [HttpGet("NpaginationEncode")]

        public async Task<IReadOnlyList<FacturacionClDto>> GetAllEncodeFacturacionCl()
        {

            var participantes = await _factClRepository.GetAllAsync();

            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_FACTCLDATA>, IReadOnlyList<FacturacionClDto>>(participantes);
            return maping;
        }
        /// <summary>
        /// obtener sin paginacion y decodificado
        /// </summary>

        [HttpGet("NpaginationDecode")]

        public async Task<IReadOnlyList<FacturacionClDto>> GetAllDecodeFacturacionCl()
        {

            var encodeList = await _factClRepository.GetAllAsync();
            List<REACT_TRGNS_FACTCLDATA> listDecode = encodeList.Select(listitem =>
                       new REACT_TRGNS_FACTCLDATA
                       {
                           ID = listitem.ID,
                           IdParticipante = listitem.IdParticipante,
                           Usuario64 = listitem.Usuario64 != null ? DecodeToString(listitem.Usuario64):null,
                           RUT64 = listitem.RUT64 != null ? DecodeToString(listitem.RUT64) : null,
                           Clave64 = listitem.Clave64 != null ? DecodeToString(listitem.Clave64) : null,
                           Puerto64 = listitem.Puerto64 != null ? DecodeToString(listitem.Puerto64) : null,
                           IncluyeLink64 = listitem.IncluyeLink64 != null ? DecodeToString(listitem.IncluyeLink64) : null,
                           UsuarioTest = listitem.UsuarioTest != null ? DecodeToString(listitem.UsuarioTest) : null,
                           ClaveTest = listitem.ClaveTest != null ? DecodeToString(listitem.ClaveTest) : null,
                           RutTest = listitem.RutTest != null ? DecodeToString(listitem.RutTest) : null,
                           Phabilitado = listitem.Phabilitado,
                       }).ToList();
            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_FACTCLDATA>, IReadOnlyList<FacturacionClDto>>(listDecode);
            return maping;
        }
        /// <summary>
        /// agregar la facturacion cl 
        /// </summary>

        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarFacturacion(AgregarFactCl agregarFactCl)
        {
            //string EncodeToBase64(string input){
            //    var output = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            //    return output;
            //}

            var datos = await _factClRepository.GetAllAsync();



            var busqueda = datos.FirstOrDefault(i => i.IdParticipante == agregarFactCl.IdParticipante);
            if (busqueda != null)
            {
                return BadRequest(new CodeErrorResponse(400, "La factura cl ya existe"));
            }
            var facturacioncl = new REACT_TRGNS_FACTCLDATA
            {
                IdParticipante = agregarFactCl.IdParticipante,
                Usuario64 = agregarFactCl.Usuario64 != null ? EncodeToBase64(agregarFactCl.Usuario64):null,
                RUT64 = agregarFactCl.RUT64 != null ? EncodeToBase64(agregarFactCl.RUT64):null,
                Clave64 = agregarFactCl.Clave64 != null ? EncodeToBase64(agregarFactCl.Clave64):null,
                Puerto64 = agregarFactCl.Puerto64 != null ? EncodeToBase64(agregarFactCl.Puerto64):null,
                IncluyeLink64 = agregarFactCl.IncluyeLink64 != null ? EncodeToBase64(agregarFactCl.IncluyeLink64): null,
                UsuarioTest = agregarFactCl.UsuarioTest != null ? EncodeToBase64(agregarFactCl.UsuarioTest):null,
                ClaveTest = agregarFactCl.ClaveTest != null ?  EncodeToBase64( agregarFactCl.ClaveTest):null,
                RutTest = agregarFactCl.RutTest != null ? EncodeToBase64(agregarFactCl.RutTest):null,
                Phabilitado = agregarFactCl.Phabilitado,

            };

            if (!await _factClRepository.SaveBD(facturacioncl))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha logrado agregar Facturacion cl"));
            }
            return Ok();
        }
        /// <summary>
        /// Actualizar la facturacion cl 
        /// </summary>

        [HttpPost("Actualizar")]
        public async Task<IActionResult> ActualizarFacturacion(AgregarFactCl agregarFactCl)
        {
            //string EncodeToBase64(string input){
            //    var output = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
            //    return output;
            //}

            var datos = await _factClRepository.GetAllAsync();



            var busqueda = datos.FirstOrDefault(i => i.IdParticipante == agregarFactCl.IdParticipante);
            if (busqueda == null)
            {
                return BadRequest(new CodeErrorResponse(400, "La factura cl no existe"));
            }

            busqueda.Usuario64 = agregarFactCl.Usuario64 != null ? EncodeToBase64(agregarFactCl.Usuario64) : busqueda.Usuario64;
            busqueda.RUT64 = agregarFactCl.RUT64 != null ? EncodeToBase64(agregarFactCl.RUT64) : busqueda.RUT64;
            busqueda.Clave64 = agregarFactCl.Clave64 != null ? EncodeToBase64(agregarFactCl.Clave64) : busqueda.Clave64;
            busqueda.Puerto64 = agregarFactCl.Puerto64 != null ? EncodeToBase64(agregarFactCl.Puerto64) : busqueda.Puerto64;
            busqueda.IncluyeLink64 = agregarFactCl.IncluyeLink64 != null ? EncodeToBase64(agregarFactCl.IncluyeLink64) : busqueda.IncluyeLink64;
            busqueda.UsuarioTest = agregarFactCl.UsuarioTest != null ? EncodeToBase64(agregarFactCl.UsuarioTest) : busqueda.UsuarioTest;
            busqueda.ClaveTest = agregarFactCl.ClaveTest != null ? EncodeToBase64(agregarFactCl.ClaveTest) : busqueda.ClaveTest;
            busqueda.RutTest = agregarFactCl.RutTest != null ? EncodeToBase64(agregarFactCl.RutTest) : busqueda.RutTest;
            busqueda.Phabilitado = agregarFactCl.Phabilitado;

            if (!await _factClRepository.UpdateeAsync(busqueda))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha logrado agregar Facturacion cl"));
            }
            return Ok();
        }
        /// <summary>
        /// obtener una facturacion especifica por id de participante
        /// </summary>

        [HttpGet("FacturacionBy/{id}")]

        public async Task<ActionResult<FacturacionClDto>> GetDecodeFacturacionCl(int id)
        {

            var encodeList = await _factClRepository.GetAllAsync();
            var busqueda = encodeList.FirstOrDefault(i => i.IdParticipante == id);

            if (busqueda == null)
            {
                return NotFound(new CodeErrorResponse(404, "la facturacion no existe"));
            }
            else
            {
                return new FacturacionClDto
                {
                    ID = busqueda.ID,
                    IdParticipante = busqueda.IdParticipante,
                    Usuario64 = busqueda.Usuario64 != null ? DecodeToString(busqueda.Usuario64) : null,
                    RUT64 = busqueda.RUT64 != null ? DecodeToString(busqueda.RUT64) : null,
                    Clave64 = busqueda.Clave64 != null ? DecodeToString(busqueda.Clave64) : null,
                    Puerto64 = busqueda.Puerto64 != null ? DecodeToString(busqueda.Puerto64) : null,
                    IncluyeLink64 = busqueda.IncluyeLink64 != null ? DecodeToString(busqueda.IncluyeLink64) : null,
                    UsuarioTest = busqueda.UsuarioTest != null ? DecodeToString(busqueda.UsuarioTest) : null,
                    ClaveTest = busqueda.ClaveTest != null ? DecodeToString(busqueda.ClaveTest) : null,
                    RutTest = busqueda.RutTest != null ? DecodeToString(busqueda.RutTest) : null,
                    Phabilitado = busqueda.Phabilitado,
                };
            }
           
        }
    }
}
