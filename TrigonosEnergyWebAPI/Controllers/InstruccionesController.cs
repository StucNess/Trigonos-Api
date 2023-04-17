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
        private readonly IGenericRepository<TRGNS_H_Datos_Facturacion> _historificacionInstruccionesRepository;
        //private readonly IGenericRepository<Patch_TRGNS_Datos_Facturacion> _instruccionessRepository;
        private readonly IMapper _mapper;

        public InstruccionesController(IGenericRepository<TRGNS_H_Datos_Facturacion> historificacionInstruccionesRepository, IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository/*, IGenericRepository<Patch_TRGNS_Datos_Facturacion> instruccionessRepository*/, IMapper mapper, IGenericRepository<CEN_payment_matrices> matricesRepository)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
            _matricesRepository = matricesRepository;
            _historificacionInstruccionesRepository = historificacionInstruccionesRepository;
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
            var bd = await _instruccionesRepository.GetByClienteIDAsync(spec);
            var bdh = new TRGNS_H_Datos_Facturacion();

            bdh.id_instruction= id;
            bdh.date = DateTime.Now;
            bdh.emission_status_old = 0;
            bdh.reception_status_old = 0;
            bdh.payment_status_old = 0;
            bdh.aceptation_status_old = 0;
            bdh.emission_date_old = new DateTime(1999,01,01);
            bdh.reception_date_old = new DateTime(1999,01,01);
            bdh.payment_date_old = new DateTime(1999,01,01);
            bdh.aceptation_date_old = new DateTime(1999,01,01);
            bdh.tipo_instruction_old = 0;
            bdh.folio_old = 0;
            bdh.emission_status_new = 0;
            bdh.reception_status_new = 0;
            bdh.payment_status_new = 0;
            bdh.aceptation_status_new = 0;
            bdh.emission_date_new = new DateTime(1999,01,01);
            bdh.reception_date_new = new DateTime(1999,01,01);
            bdh.payment_date_new = new DateTime(1999,01,01);
            bdh.aceptation_date_new = new DateTime(1999,01,01);
            bdh.tipo_instruction_new = 0;
            bdh.folio_new = 0;
            var condicional = 0;
            bdh.editor = parametros.Editor;
            //if (parametros.FechaAceptacion != bd.Fecha_aceptacion && parametros.FechaAceptacion != null)
            //{
            //    bdh.aceptation_date_old = bd.Fecha_aceptacion;
            //    bd.Fecha_aceptacion = parametros.FechaAceptacion;
            //    bdh.aceptation_date_new = parametros.FechaAceptacion;
            //}

            if (parametros.EstadoEmision != bd.Estado_emision && parametros.EstadoEmision != null)
            {
                bdh.emission_status_old = bd.Estado_emision;
                bd.Estado_emision = parametros.EstadoEmision;
                bdh.emission_status_new = parametros.EstadoEmision;
                condicional = 1;

            }
            if (parametros.EstadoRecepcion != bd.Estado_recepcion && parametros.EstadoRecepcion != null)
            {
                bdh.reception_status_old = bd.Estado_recepcion;
                bd.Estado_recepcion = parametros.EstadoRecepcion;
                bdh.reception_status_new = parametros.EstadoRecepcion;
                condicional = 1;
            }

            if (parametros.EstadoPago != bd.Estado_pago && parametros.EstadoPago != null)
            {
                bdh.payment_status_old = bd.Estado_pago;
                bd.Estado_pago = parametros.EstadoPago;
                bdh.payment_status_new = bd.Estado_pago;
                condicional = 1;
            }

            if (parametros.EstadoAceptacion != bd.Estado_aceptacion && parametros.EstadoAceptacion != null)
            {
                bdh.aceptation_status_old = bd.Estado_aceptacion;
                bd.Estado_aceptacion = parametros.EstadoAceptacion;
                bdh.emission_status_new = parametros.EstadoAceptacion;
            }

            if (parametros.FechaEmision != bd.Fecha_emision && parametros.FechaEmision != null)
            {
                bdh.emission_date_old = bd.Fecha_emision;
                bd.Fecha_emision = parametros.FechaEmision;
                bdh.emission_date_new = parametros.FechaEmision;
                bdh.emission_status_old = bd.Estado_emision;
                bd.Estado_emision = 2;
                bdh.emission_status_new = 2;
                condicional = 1;
            }

            if (parametros.FechaAceptacion != bd.Fecha_aceptacion && parametros.FechaAceptacion != null)
            {
                bdh.aceptation_date_old = bd.Fecha_aceptacion;
                bd.Fecha_aceptacion = parametros.FechaAceptacion;
                bdh.aceptation_date_new = parametros.FechaAceptacion;
                condicional = 1;
            }

            if (parametros.FechaPago != bd.Fecha_pago && parametros.FechaPago != null)
            {
                bdh.payment_date_old = bd.Fecha_pago;
                bd.Fecha_pago = parametros.FechaPago;
                bdh.payment_date_new = parametros.FechaPago;
                bdh.payment_status_old = bd.Estado_pago;
                bd.Estado_pago = 2;
                bdh.payment_status_new =2;
                condicional = 1;
            }
            if (parametros.FechaRecepcion != bd.Fecha_recepcion && parametros.FechaRecepcion != null)
            {
                bdh.reception_date_old = bd.Fecha_recepcion;
                bd.Fecha_recepcion = parametros.FechaRecepcion;
                bdh.reception_date_new = parametros.FechaRecepcion;
                bdh.aceptation_date_old = bd.Fecha_aceptacion;
                bd.Fecha_aceptacion = parametros.FechaRecepcion;
                bdh.aceptation_date_new = parametros.FechaRecepcion;

                bdh.reception_status_old = bd.Estado_recepcion;
                bd.Estado_recepcion = 1;
                bdh.reception_status_new = 1;
                bdh.aceptation_status_old = bd.Estado_aceptacion;
                bd.Estado_aceptacion = 1;
                bdh.emission_status_new = 1;

                condicional = 1;
            }
            if (parametros.TipoInstructions != bd.tipo_instructions && parametros.TipoInstructions != null)
            {
                bdh.tipo_instruction_old = bd.tipo_instructions;
                bd.tipo_instructions = parametros.TipoInstructions;
                bdh.tipo_instruction_new = parametros.TipoInstructions;
                condicional = 1;
            }

            if (parametros.Folio != bd.Folio && parametros.Folio != null)
            {
                bdh.folio_old = bd.Folio;
                bd.Folio = parametros.Folio;
                bdh.folio_new = parametros.Folio;
                condicional = 1;
            }

            var guardar1 = await _instruccionesRepository.UpdateeAsync(bd);
            //if (!await _instruccionesRepository.UpdateeAsync(bd))
            //{
            //    return StatusCode(500);
            //}
            if (parametros.Editor != "Masivo")
            {
                if(condicional == 1)
                {
                 var guardar = await _historificacionInstruccionesRepository.SaveBD(bdh);
                }
                

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
        [Route("/ssFiltros")]
        public async Task<ActionResult<IReadOnlyList<ssFiltros>>> ssFiltros(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        {
            //.Server.ScriptTimeout = 300;

            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var producto1 = producto.DistinctBy(p => p.CEN_instruction.Payment_matrix_natural_key).ToList();
            var Carta = producto.DistinctBy(p => p.CEN_instruction.cEN_Payment_Matrices.Letter_code).ToList();
            var CodRef = producto.DistinctBy(p => p.CEN_instruction.cEN_Payment_Matrices.Reference_code).ToList();
            List<string> listConcept = new List<string>();
            List<string> listCarta = new List<string>();
            List<string> listCodRef = new List<string>();
            foreach (var item in CodRef)
            {
                var co2 = item.CEN_instruction.cEN_Payment_Matrices.Reference_code;
                listCodRef.Add(co2);

            }
            foreach (var item in Carta)
            {
                var co1 = item.CEN_instruction.cEN_Payment_Matrices.Letter_code;
                listCarta.Add(co1);

            }
            foreach (var item in producto1)
            {
                var co = item.CEN_instruction.Payment_matrix_natural_key;
                listConcept.Add(co);

            }

            return Ok(
                new ssFiltros
                {
                    label = listConcept,
                    Carta = listCarta,
                    CodRef = listCodRef

                }
                );


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
