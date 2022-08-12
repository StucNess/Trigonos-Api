using AutoMapper;
using Core.Entities;
using Core.EntitiesPatch;
using Core.Interface;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{

    public class InstruccionesController : BaseApiController
    {
        private readonly IGenericRepository<TRGNS_Datos_Facturacion> _instruccionesRepository;
        //private readonly IGenericRepository<Patch_TRGNS_Datos_Facturacion> _instruccionessRepository;
        private readonly IMapper _mapper;

        public InstruccionesController(IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository/*, IGenericRepository<Patch_TRGNS_Datos_Facturacion> instruccionessRepository*/, IMapper mapper)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
            //_instruccionessRepository = instruccionessRepository;
        }


        //[HttpGet]
        //public async Task<ActionResult<List<Patch_TRGNS_Datos_Facturacion>>> GetParticipantes(/*[FromQuery] InstruccionesSpecificationParams parametros*/)
        //{
        //    //var spec = new InstruccionesRelationSpecification(parametros);
        //    var producto = await _instruccionessRepository.GetAllAsync();
        //    //return Ok(_mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<Datos_Facturacion_DTO>>(producto));
        //    return Ok(producto);
        //}
        //[HttpGet("{id}")]

        //public async Task<ActionResult<TRGNS_Datos_Facturacion>> GetParticipante(int id, [FromQuery] InstruccionesSpecificationParams parametros)
        //{

        //    var spec = new PruebaParams(id/*, parametros*/);
        //    var prueba = await _instruccionesRepository.GetByClienteIDAsync(spec);
        //    prueba.Estado_emision = parametros.Folio;
        //    prueba.Estado_pago = facturacion.Estado_pago;
        //    prueba.Estado_aceptacion = facturacion.Estado_aceptacion;
        //    prueba.Estado_recepcion = facturacion.Estado_recepcion;
        //    prueba.Fecha_recepcion = facturacion.Fecha_recepcion;
        //    prueba.Fecha_emision = facturacion.Fecha_emision;
        //    prueba.Fecha_pago = facturacion.Fecha_pago;
        //    prueba.Fecha_aceptacion = facturacion.Fecha_aceptacion;
        //    prueba.tipo_instructions = facturacion.tipo_instructions;
        //    prueba.Folio = facturacion.Folio;
        //    //return _mapper.Map<CEN_Participants, ParticipantesDTO>(producto);
        //    return prueba;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagination<InstruccionesDTO>>> GetInstrucciones(int id, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var specCount = new InstruccionesForCountingSpecification(id, parametros);
            var totalinstrucciones = await _instruccionesRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<InstruccionesDTO>>(producto);


            return Ok(
                new Pagination<InstruccionesDTO>
                {
                    count = totalinstrucciones,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,



                }
                );
        }
        [HttpPatch]
        public async Task<IActionResult> Updatee(int id, [FromQuery] PatchInstruccionesParams parametros)
        {
            //if (facturacion == null || id != facturacion.id_instructions)
            //{
            //    return BadRequest(ModelState);
            //}
            //var spec = new InstruccionesRelationSpecification(id, parametros);
            //var prueba = _instruccionesRepository.GetByClienteIDAsync(spec);
            var spec = new PruebaParams(id/*, parametros*/);
            var prueba = await _instruccionesRepository.GetByClienteIDAsync(spec);
            if (parametros.EstadoEmision != null)
            {
                prueba.Estado_emision = parametros.EstadoEmision;
            }
            if (parametros.EstadoRecepcion != null)
            {
                prueba.Estado_recepcion = parametros.EstadoRecepcion;
            }
            if (parametros.EstadoRecepcion != null)
            {
                prueba.Estado_recepcion = parametros.EstadoRecepcion;
            }
            if (parametros.EstadoPago != null)
            {
                prueba.Estado_pago = parametros.EstadoPago;
            }
            if (parametros.EstadoAceptacion != null)
            {
                prueba.Estado_aceptacion = parametros.EstadoAceptacion;
            }
            if (parametros.FechaEmision != null)
            {
                prueba.Fecha_emision = parametros.FechaEmision;
            }
            if (parametros.FechaRecepcion != null)
            {
                prueba.Fecha_recepcion = parametros.FechaRecepcion;
            }
            if (parametros.FechaPago != null)
            {
                prueba.Fecha_pago = parametros.FechaPago;
            }
            if (parametros.FechaAceptacion != null)
            {
                prueba.Fecha_aceptacion = parametros.FechaAceptacion;
            }
            if (parametros.TipoInstructions != null)
            {
                prueba.tipo_instructions = parametros.TipoInstructions;
            }

            if (parametros.Folio != null)
            {
                prueba.Folio = parametros.Folio;
            }
            
            
            
            
            

            //prueba. = id;
            //prueba.Estado_emision = facturacion.Estado_emision;
            //prueba.Estado_pago = facturacion.Estado_pago;
            //prueba.Estado_aceptacion = facturacion.Estado_aceptacion;
            //prueba.Estado_recepcion = facturacion.Estado_recepcion;
            //prueba.Fecha_recepcion = facturacion.Fecha_recepcion;
            //prueba.Fecha_emision = facturacion.Fecha_emision;
            //prueba.Fecha_pago = facturacion.Fecha_pago;
            //prueba.Fecha_aceptacion = facturacion.Fecha_aceptacion;
            //prueba.tipo_instructions = facturacion.tipo_instructions;
            //prueba.Folio = facturacion.Folio;
            //var pro = _mapper.Map<Patch_TRGNS_Datos_Facturacion>(facturacion);
            if (!await _instruccionesRepository.UpdateeAsync(prueba))
            {
                return StatusCode(500);
            }
            return NoContent();
        }
        //private decimal ConvertToDecimal(int v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
