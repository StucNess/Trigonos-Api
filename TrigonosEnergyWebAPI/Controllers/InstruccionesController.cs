using AutoMapper;
using Core.Entities;

using Core.Interface;
using Core.Specifications;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIInstrucciones")]

    public class InstruccionesController : BaseApiController
    {
        private readonly IGenericRepository<TRGNS_Datos_Facturacion> _instruccionesRepository;
        private readonly IGenericRepository<CEN_payment_matrices> _matricesRepository;
        //private readonly IGenericRepository<Patch_TRGNS_Datos_Facturacion> _instruccionessRepository;
        private readonly IMapper _mapper;

        public InstruccionesController(IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository/*, IGenericRepository<Patch_TRGNS_Datos_Facturacion> instruccionessRepository*/, IMapper mapper, IGenericRepository<CEN_payment_matrices> matricesRepository)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
            _matricesRepository = matricesRepository;
            //_instruccionessRepository = instruccionessRepository;
        }
        /// <summary>
        /// Obtener la glosa de todas las instrucciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Glosa")]
        public async Task<ActionResult<IReadOnlyList<CEN_Participants>>> PRUEBA1()
        {
            var datos = await _matricesRepository.GetAllAsync();
            var maping = _mapper.Map<IReadOnlyList<CEN_payment_matrices>, IReadOnlyList<FolioDto>>(datos);
            return Ok(maping);
        }

        /// <summary>
        /// Obtener todas las instrucciones de un participante
        /// </summary>
        /// <param name="id"> ID del participante</param>
        /// <param name="parametros.FechaEmision"> gola</param>
        /// 
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pagination<InstruccionesDTO>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
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
        /// <summary>
        /// Modificar datos de una instruccion especifica
        /// </summary>
        /// <param name="id">ID de la instruccion</param>
        /// <param name="parametros"> hola </param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Updatee(int id, [FromQuery] PatchInstruccionesParams parametros)
        {

            var spec = new PruebaParams(id);
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

            if (!await _instruccionesRepository.UpdateeAsync(prueba))
            {
                return StatusCode(500);
            }
            return NoContent();
        }
        [HttpGet]
        [Route("/sFiltros")]
        public async Task<ActionResult<IReadOnlyList<sFiltros>>> sFiltros(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id,pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            //var specCount = new InstruccionesForCountingSpecification(id, parametros);
            //var totalinstrucciones = await _instruccionesRepository.CountAsync(specCount);
            //var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);
            var producto1 = producto.DistinctBy(p => p.CEN_instruction.Payment_matrix_natural_key).ToList();
            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltros>>(producto1);
            //var probando = data
            return Ok(data);
            //return Ok(
            //    new Pagination<sFiltros>
            //    {
            //        count = totalinstrucciones,
            //        Data = data,

            //    }
            //    );


    }
        [HttpGet]
        [Route("/sFiltrosRutCreditor")]
        public async Task<ActionResult<IReadOnlyList<sFiltros>>> sFiltrosRutCreditor(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var producto1 = producto.DistinctBy(a => a.CEN_instruction.Participants_creditor.Rut).ToList();
            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltrosRutCreditor>>(producto1);

            return Ok(data);

        }
        [HttpGet]
        [Route("/sFiltrosRutDeudor")]
        public async Task<ActionResult<IReadOnlyList<sFiltros>>> sFiltrosRutDeudor(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var producto1 = producto.DistinctBy(a => a.CEN_instruction.Participants_debtor.Rut).ToList();
            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltrosRutDeudor>>(producto1);
            return Ok(data);



        }
        [HttpGet]
        [Route("/sFiltrosNameCreditor")]
        public async Task<ActionResult<IReadOnlyList<sFiltros>>> sFiltrosNameCreditor(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var producto1 = producto.DistinctBy(a => a.CEN_instruction.Participants_creditor.Business_Name).ToList();
            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltrosNameCreditor>>(producto1);
            return Ok(data);



        }
        [HttpGet]
        [Route("/sFiltrosNameDebtor")]
        public async Task<ActionResult<IReadOnlyList<sFiltros>>> sFiltrosNameDebtor(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var producto1 = producto.DistinctBy(a => a.CEN_instruction.Participants_debtor.Business_Name).ToList();
            var data = _mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltrosNameDebtor>>(producto1);
            return Ok(data);



        }
    } }
