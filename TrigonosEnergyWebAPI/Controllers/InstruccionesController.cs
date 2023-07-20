using AutoMapper;
using AutoMapper.Execution;
using Core.Entities;

using Core.Interface;
using Core.Specifications;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Facturacion.cl;
using LogicaTrigonos.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIInstrucciones")]

    public class InstruccionesController : BaseApiController
    {
        private readonly IGenericRepository<REACT_TRGNS_Datos_Facturacion> _instruccionesRepository;
        private readonly IGenericRepository<REACT_CEN_instructions_Def> _instruccionesDefRepository;
        private readonly IGenericRepository<REACT_CEN_payment_matrices> _matricesRepository;
        private readonly IGenericRepository<REACT_TRGNS_H_Datos_Facturacion> _historificacionInstruccionesRepository;
        private readonly IGenericRepository<REACT_CEN_Participants> _participantesRepository;
        private readonly IGenericRepository<REACT_TRGNS_FACTCLDATA> _factClRepository;

        private readonly IGenericRepository<REACT_TRGNS_Excel_History> _excelHistoryRepository;
        //private readonly IGenericRepository<Patch_TRGNS_Datos_Facturacion> _instruccionessRepository;
        private readonly IMapper _mapper;
        private readonly TrigonosDBContext _context;
        public InstruccionesController(IGenericRepository<REACT_TRGNS_FACTCLDATA> factClRepository, IGenericRepository<REACT_CEN_Participants> participantesRepository, TrigonosDBContext context, IGenericRepository<REACT_TRGNS_Excel_History> excelHistoryRepository, IGenericRepository<REACT_CEN_instructions_Def> instruccionesDefRepository, IGenericRepository<REACT_TRGNS_H_Datos_Facturacion> historificacionInstruccionesRepository, IGenericRepository<REACT_TRGNS_Datos_Facturacion> instruccionesRepository/*, IGenericRepository<Patch_TRGNS_Datos_Facturacion> instruccionessRepository*/, IMapper mapper, IGenericRepository<REACT_CEN_payment_matrices> matricesRepository)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
            _matricesRepository = matricesRepository;
            _historificacionInstruccionesRepository = historificacionInstruccionesRepository;
            _instruccionesDefRepository = instruccionesDefRepository;
            _excelHistoryRepository = excelHistoryRepository;
            _context = context;
            _participantesRepository = participantesRepository;
            _factClRepository = factClRepository;
        }
        static TimeZoneInfo zonaHorariaChile = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
        DateTime fechaHoraActualChile = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, zonaHorariaChile);
        string DecodeToString(string input)
        {
            var output = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input));
            return output;
        }
        [HttpGet]
        [Route("/excelHistory")]
        public async Task<ActionResult<IReadOnlyList<excelHistoryDto>>> excelHistory([FromQuery] excelHistoryParams parametros)
        {
            var spec = new excelHistorySpecification(parametros);
            var producto = await _excelHistoryRepository.GetAllAsync(spec);
            //var producto1 = producto.DistinctBy(a => a.Participants_creditor.Rut).ToList();




            var specCount = new excelHistoryForCounting(parametros);
            var totalinstrucciones = await _excelHistoryRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_Excel_History>, IReadOnlyList<excelHistoryDto>>(producto);


            return Ok(
                new Pagination<excelHistoryDto>
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
        /// Obtener la glosa de todas las instrucciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Glosa")]
        public async Task<ActionResult<IReadOnlyList<REACT_CEN_Participants>>> PRUEBA1()
        {
            var datos = await _matricesRepository.GetAllAsync();
            var maping = _mapper.Map<IReadOnlyList<REACT_CEN_payment_matrices>, IReadOnlyList<FolioDto>>(datos);
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

            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_Datos_Facturacion>, IReadOnlyList<InstruccionesDTO>>(producto);


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

        [HttpGet]
        [Route("InstruccionesDef/{id}")]
        [ProducesResponseType(200, Type = typeof(Pagination<InstruccionesDefDTO>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Pagination<InstruccionesDefDTO>>> GetInstruccionesDef(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var spec = new InstruccionesDefRelationSpecification(id, parametros);
            var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            var specCount = new InstruccionesDefForCountingSpecification(id, parametros);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<InstruccionesDefDTO>>(instrucciones);


            return Ok(
                new Pagination<InstruccionesDefDTO>
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
            var bd = await _instruccionesDefRepository.GetByClienteIDAsync(spec);
            var bdh = new REACT_TRGNS_H_Datos_Facturacion();

            bdh.id_instruction = id;
            bdh.date = DateTime.Now;
            bdh.emission_status_old = 0;
            bdh.reception_status_old = 0;
            bdh.payment_status_old = 0;
            bdh.aceptation_status_old = 0;
            bdh.emission_date_old = new DateTime(1999, 01, 01);
            bdh.reception_date_old = new DateTime(1999, 01, 01);
            bdh.payment_date_old = new DateTime(1999, 01, 01);
            bdh.aceptation_date_old = new DateTime(1999, 01, 01);
            bdh.tipo_instruction_old = 0;
            bdh.folio_old = 0;
            bdh.emission_status_new = 0;
            bdh.reception_status_new = 0;
            bdh.payment_status_new = 0;
            bdh.aceptation_status_new = 0;
            bdh.emission_date_new = new DateTime(1999, 01, 01);
            bdh.reception_date_new = new DateTime(1999, 01, 01);
            bdh.payment_date_new = new DateTime(1999, 01, 01);
            bdh.aceptation_date_new = new DateTime(1999, 01, 01);
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
                bdh.payment_status_new = 2;
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

            var guardar1 = await _instruccionesDefRepository.UpdateeAsync(bd);
            //if (!await _instruccionesRepository.UpdateeAsync(bd))
            //{
            //    return StatusCode(500);
            //}
            if (parametros.Editor != "Masivo")
            {
                if (condicional == 1)
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
            var spec = new InstruccionesRelationSpecification(id, pa, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            //var specCount = new InstruccionesForCountingSpecification(id, parametros);
            //var totalinstrucciones = await _instruccionesRepository.CountAsync(specCount);
            //var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);
            var producto1 = producto.DistinctBy(p => p.CEN_instruction.Payment_matrix_natural_key).ToList();
            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_Datos_Facturacion>, IReadOnlyList<sFiltros>>(producto1);
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
        //[HttpGet]
        //[Route("/ssFiltros")]
        //public async Task<ActionResult<IReadOnlyList<ssFiltros>>> ssFiltros(int id, int pa, [FromQuery] InstruccionesSpecificationParams parametros)
        //{
        //    //.Server.ScriptTimeout = 300;

        //    var spec = new InstruccionesRelationSpecification(id, pa, parametros);
        //    var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
        //    var producto1 = producto.DistinctBy(p => p.CEN_instruction.Payment_matrix_natural_key).ToList();
        //    var Carta = producto.DistinctBy(p => p.CEN_instruction.cEN_Payment_Matrices.Letter_code).ToList();
        //    var CodRef = producto.DistinctBy(p => p.CEN_instruction.cEN_Payment_Matrices.Reference_code).ToList();
        //    List<string> listConcept = new List<string>();
        //    List<string> listCarta = new List<string>();
        //    List<string> listCodRef = new List<string>();
        //    foreach (var item in CodRef)
        //    {
        //        var co2 = item.CEN_instruction.cEN_Payment_Matrices.Reference_code;
        //        listCodRef.Add(co2);

        //    }
        //    foreach (var item in Carta)
        //    {
        //        var co1 = item.CEN_instruction.cEN_Payment_Matrices.Letter_code;
        //        listCarta.Add(co1);

        //    }
        //    foreach (var item in producto1)
        //    {
        //        var co = item.CEN_instruction.Payment_matrix_natural_key;
        //        listConcept.Add(co);

        //    }

        //    return Ok(
        //        new ssFiltros
        //        {
        //            label = listConcept,
        //            Carta = listCarta,
        //            CodRef = listCodRef

        //        }
        //        );


        //}
        [HttpGet]
        [Route("/ssFiltros")]
        public async Task<ActionResult<IReadOnlyList<ssFiltros>>> ssFiltros(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //.Server.ScriptTimeout = 300;


            var spec = new InstruccionesDefRelationSpecification(id, pa, parametros);
            var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            var Concepto = instrucciones.DistinctBy(p => p.Payment_matrix_natural_key).ToList();
            var Carta = instrucciones.DistinctBy(p => p.cEN_Payment_Matrices.Letter_code).ToList();
            var CodRef = instrucciones.DistinctBy(p => p.cEN_Payment_Matrices.Reference_code).ToList();

            var listConcept = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<ConceptoMapper>>(Concepto);
            var listCarta = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<CartaMapper>>(Carta);
            var listCodRef = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<CodRefMapper>>(CodRef);


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
        [Route("/NumberFilter")]
        public async Task<ActionResult<int>> NumberFilter(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specCount = new InstruccionesDefForCountingSpecification(id, parametros);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(specCount);

            return Ok(totalinstrucciones);



        }
        [HttpGet]
        [Route("/GetFiltersCCC")]
        public async Task<ActionResult<Pagination<FiltroCCCDto>>> GetFiltersCCC(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            var spec = new InstruccionesDefRelationSpecification(id, pa, parametros);
            var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var Concepto = instrucciones.DistinctBy(p => p.Payment_matrix_natural_key).ToList();
            //var Carta = instrucciones.DistinctBy(p => p.cEN_Payment_Matrices.Letter_code).ToList();
            //var CodRef = instrucciones.DistinctBy(p => p.cEN_Payment_Matrices.Reference_code).ToList();
            var specCount = new InstruccionesDefForCountingSpecification(id, parametros);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(specCount);
            var lista = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<FiltroCCCDto>>(instrucciones);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            return Ok(
                new Pagination<FiltroCCCDto>
                {
                    count = totalinstrucciones,
                    Data = lista,
                    PageCount = totalPages,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,
                }
                );
        }
        /// <summary>
        /// Obtener la cantidad del concepto para luego iterar y saber cuantas llamadas hacer al filtro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/CountingConcept")]
        public async Task<ActionResult<int>> CountingConcept(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Payment_matrix_natural_key");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            return count.Count();

        }
        /// <summary>
        /// Obtener la cantidad del Cod Ref para luego iterar y saber cuantas llamadas hacer al filtro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/CountingCodRef")]
        public async Task<ActionResult<int>> CountingCodRef(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "cEN_Payment_Matrices.Reference_code");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);

            if (count != null)
            {
                return Ok(count.Count());
            }
            else
            {
                return 0;
            }


        }
        /// <summary>
        /// Obtener la cantidad de la carta para luego iterar y saber cuantas llamadas hacer al filtro
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/CountingCarta")]
        public async Task<ActionResult<int>> CountingCarta(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "cEN_Payment_Matrices.Letter_code");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);

            if (count != null)
            {
                return Ok(count.Count());
            }
            else
            {
                return 0;
            }


        }
        /// <summary>
        /// Obtener El concepto de un participante especifico
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/ConceptFilter")]
        public async Task<ActionResult<List<string>>> ConceptFilter(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
                    (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.Payment_matrix_natural_key).Distinct().ToList();
            return Ok(
                List
                );
            ////ERROR AQUI NO FUNCIONA LA CONDICION PUEDE SER EL COUNT
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Payment_matrix_natural_key");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Payment_matrix_natural_key");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);

            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<ConceptoMapper>>(instrucciones);

            //return Ok(
            //    new Pagination<ConceptoMapper>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );





        }

        /// <summary>
        /// Obtener El concepto de un participante especifico
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/CodRefFilter")]
        public async Task<ActionResult<List<string>>> CodRefFilter(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
                    (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.cEN_Payment_Matrices.Reference_code).Distinct().ToList();
            return Ok(
                List
                );
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "cEN_Payment_Matrices.Reference_code");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "cEN_Payment_Matrices.Reference_code");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<CodRefMapper>>(instrucciones);
            //return Ok(
            //    new Pagination<CodRefMapper>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );
        }

        /// <summary>
        /// Obtener la carta de un participante especifico
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/CartaFilter")]
        public async Task<ActionResult<Pagination<CartaMapper>>> CartaFilter(int id, int pa, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.cEN_Payment_Matrices.Letter_code).Distinct().ToList();
            return Ok(
                List
                );
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "cEN_Payment_Matrices.Letter_code");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "cEN_Payment_Matrices.Letter_code");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<CartaMapper>>(instrucciones);
            //return Ok(
            //    new Pagination<CartaMapper>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );
        }
        [HttpGet]
        [Route("/CountingRutAcreedor")]
        public async Task<ActionResult<int>> CountingRutAcreedor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_creditor.Rut");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            return count.Count();

        }
        [HttpGet]
        [Route("/CountingNombreAcreedor")]
        public async Task<ActionResult<int>> CountingNombreAcreedor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_creditor.Business_Name");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            return count.Count();

        }
        [HttpGet]
        [Route("/CountingRutDeudor")]
        public async Task<ActionResult<int>> CountingRutDeudor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_debtor.Rut");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            return count.Count();

        }
        [HttpGet]
        [Route("/CountingNombreDeudor")]
        public async Task<ActionResult<int>> CountingNombreDeudor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {

            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_debtor.Business_Name");
            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            return count.Count();

        }

        [HttpGet]
        [Route("/sFiltrosRutCreditor")]
        public async Task<ActionResult<List<string>>> sFiltrosRutCreditor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //Task<ActionResult<Pagination<sFiltrosRutCreditor>>>
            //Task<ActionResult<IReadOnlyList<sFiltros>>>
            //var spec = new InstruccionesDefRelationSpecification(id, 1, parametros);
            //var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var producto1 = producto.DistinctBy(a => a.Participants_creditor.Rut).ToList();
            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosRutCreditor>>(producto1);

            //return Ok(data);


            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.Participants_creditor.Rut).Distinct().ToList();
            return Ok(
                List
                );
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Participants_creditor.Rut");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_creditor.Rut");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosRutCreditor>>(instrucciones);
            //return Ok(
            //    new Pagination<sFiltrosRutCreditor>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );

        }
        [HttpGet]
        [Route("/sFiltrosRutDeudor")]
        public async Task<ActionResult<Pagination<sFiltrosRutDeudor>>> sFiltrosRutDeudor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //var spec = new InstruccionesDefRelationSpecification(id, 1, parametros);

            //var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);

            //var producto1 = producto.DistinctBy(a => a.Participants_debtor.Rut).ToList();
            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosRutDeudor>>(producto1);
            //return Ok(data);
            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.Participants_debtor.Rut).Distinct().ToList();
            return Ok(
                List
                );

            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Participants_debtor.Rut");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_debtor.Rut");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosRutDeudor>>(instrucciones);
            //return Ok(
            //    new Pagination<sFiltrosRutDeudor>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );

        }
        [HttpGet]
        [Route("/sFiltrosNameCreditor")]
        public async Task<ActionResult<List<string>>> sFiltrosNameCreditor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //var spec = new InstruccionesDefRelationSpecification(id, 1, parametros);
            //var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var producto1 = producto.DistinctBy(a => a.Participants_creditor.Business_Name).ToList();
            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameCreditor>>(producto1);
            //return Ok(data);
            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.Participants_creditor.Business_Name).Distinct().ToList();
            return Ok(
                List
                );
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Participants_creditor.Business_Name");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_creditor.Business_Name");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameCreditor>>(instrucciones);
            //return Ok(
            //    new Pagination<sFiltrosNameCreditor>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );


        }
        [HttpGet]
        [Route("/FiltNameDebtor")]
        public async Task<ActionResult<List<string>>> FiltNameDebtor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //var spec = new InstruccionesDefRelationSpecification(id, 1, parametros);
            //var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var producto1 = producto.DistinctBy(a => a.Participants_debtor.Business_Name).ToList();
            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameDebtor>>(producto1);
            //return Ok(data);
            //List<string> List = new List<string>();
            //List = _context.Set<REACT_CEN_instructions_Def>()
            //    .Where(x => (x.Creditor == id || x.Debtor == id) &&
            //       (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
            //      (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
            //       (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
            //       (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
            //       (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
            //      (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
            //      (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
            //      (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
            //      (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
            //      (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
            //      (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
            //      (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
            //       (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            //(string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
            //      (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
            //      (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
            //      (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
            //      (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
            //     (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
            //      (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
            //      (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

            //      (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
            //      (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
            //      (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
            //       &&
            //       (
            //       !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
            //       ||

            //       (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

            //       x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

            //       ||

            //       (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
            //       !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
            //       x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
            //        && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
            //       ||
            //      (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
            //       x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
            //       x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
            //        && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

            //       )
            //      ).Select(item => item.Participants_debtor.Business_Name).Distinct().ToList();
            //return Ok(
            //    List
            //    );
            var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Participants_debtor.Business_Name");
            var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_debtor.Business_Name");

            var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            var datacount = 0;
            if (count != null)
            {
                datacount = count.Count();
            }
            var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);


            var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameDebtor>>(instrucciones);
            return Ok(
                new Pagination<sFiltrosNameDebtor>
                {
                    count = datacount,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,
                }
                );

        }
        [HttpGet]
        [Route("/sFiltrosNameDebtor")]
        public async Task<ActionResult<List<string>>> sFiltrosNameDebtor(int id, [FromQuery] InstruccionesDefSpecificationParams parametros)
        {
            //var spec = new InstruccionesDefRelationSpecification(id, 1, parametros);
            //var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var producto1 = producto.DistinctBy(a => a.Participants_debtor.Business_Name).ToList();
            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameDebtor>>(producto1);
            //return Ok(data);
            List<string> List = new List<string>();
            List = _context.Set<REACT_CEN_instructions_Def>()
                .Where(x => (x.Creditor == id || x.Debtor == id) &&
                   (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (string.IsNullOrEmpty(parametros.EstadoAceptacion) || x.CEN_dte_acceptance_status.Name == parametros.EstadoAceptacion) &&
                   (string.IsNullOrEmpty(parametros.EstadoRecepcion) || x.TRGNS_dte_reception_status.Name == parametros.EstadoRecepcion) &&
                   (string.IsNullOrEmpty(parametros.EstadoEmision) || x.CEN_billing_status_type.Name == parametros.EstadoEmision) &&
                   (string.IsNullOrEmpty(parametros.EstadoPago) || x.CEN_payment_status_type.Name == parametros.EstadoPago) &&
                  (string.IsNullOrEmpty(parametros.conFolio) || x.Folio > 0) &&
                  (string.IsNullOrEmpty(parametros.NombreAcreedor) || x.Participants_creditor.Business_Name.Contains(parametros.NombreAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.NombreDeudor) || x.Participants_debtor.Business_Name.Contains(parametros.NombreDeudor)) &&
                  (string.IsNullOrEmpty(parametros.RutAcreedor) || x.Participants_creditor.Rut.Contains(parametros.RutAcreedor)) &&
                  (string.IsNullOrEmpty(parametros.RutDeudor) || x.Participants_debtor.Rut.Contains(parametros.RutDeudor)) &&
                  (string.IsNullOrEmpty(parametros.Glosa) || x.Payment_matrix_natural_key.Contains(parametros.Glosa)) &&
                  (string.IsNullOrEmpty(parametros.Concepto) || x.Payment_matrix_concept.Contains(parametros.Concepto)) &&
                   (string.IsNullOrEmpty(parametros.Carta) || x.cEN_Payment_Matrices.Letter_code.Contains(parametros.Carta)) &&
            (string.IsNullOrEmpty(parametros.CodigoRef) || x.cEN_Payment_Matrices.Reference_code.Contains(parametros.CodigoRef)) &&
                  (!parametros.FechaRecepcion.HasValue || x.Fecha_recepcion == parametros.FechaRecepcion) &&
                  (!parametros.FechaAceptacion.HasValue || x.Fecha_recepcion == parametros.FechaAceptacion) &&
                  (!parametros.FechaPago.HasValue || x.Fecha_recepcion == parametros.FechaPago) &&
                  (!parametros.FechaEmision.HasValue || x.Fecha_recepcion == parametros.FechaEmision) &&
                 (!parametros.Pagada.HasValue || x.Is_paid == parametros.Pagada) &&
                  (!parametros.Acreedor.HasValue || x.Creditor == parametros.Acreedor) &&
                  (!parametros.Deudor.HasValue || x.Debtor == parametros.Deudor) &&

                  (!parametros.MontoNeto.HasValue || x.Amount >= parametros.MontoNeto) &&
                  (!parametros.MontoBruto.HasValue || x.Amount_Gross >= parametros.MontoBruto) &&
                  (!parametros.Folio.HasValue || x.Folio == parametros.Folio)
                   &&
                   (
                   !parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue
                   ||

                   (parametros.InicioPeriodo.HasValue && !parametros.TerminoPeriodo.HasValue &&

                   x.cEN_Payment_Matrices.CEN_billing_windows.period == parametros.InicioPeriodo)

                   ||

                   (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   !x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)
                   ||
                  (parametros.TerminoPeriodo.HasValue && parametros.InicioPeriodo.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end.HasValue &&
                   x.cEN_Payment_Matrices.CEN_billing_windows.period_end <= parametros.TerminoPeriodo
                    && x.cEN_Payment_Matrices.CEN_billing_windows.period >= parametros.InicioPeriodo)

                   )
                  ).Select(item => item.Participants_debtor.Business_Name).Distinct().ToList();
            return Ok(
                List
                );
            //var spec = new InstruccionesDefRelationSpecification(id, parametros, 1, "Participants_debtor.Business_Name");
            //var instrucciones = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            //var specTotal = new InstruccionesDefRelationSpecification(id, parametros, 0, "Participants_debtor.Business_Name");

            //var count = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(specTotal);
            //var datacount = 0;
            //if (count != null)
            //{
            //    datacount = count.Count();
            //}
            //var rounded = Math.Ceiling(Convert.ToDecimal(datacount / parametros.PageSize));
            //var totalPages = Convert.ToInt32(rounded);


            //var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<sFiltrosNameDebtor>>(instrucciones);
            //return Ok(
            //    new Pagination<sFiltrosNameDebtor>
            //    {
            //        count = datacount,
            //        Data = data,
            //        PageCount = totalPages,
            //        PageIndex = parametros.PageIndex,
            //        PageSize = parametros.PageSize,
            //    }
            //    );

        }
        [HttpPost("NominasDePago")]
        public async Task<ActionResult> NominasDePago(int id, int bank, string excelName, string fechaPago, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            Console.WriteLine(fechaPago);
            int conditional = 0;
            var FechaPago = Convert.ToDateTime(fechaPago);
            List<int> numberList = new List<int>();
            var BDD = _context.Set<REACT_CEN_instructions_Def>()
                .Where(e => e.Folio > 0 && e.Amount > 9 && e.Debtor == id);
            if (bank == 4) // BCI
            {
                foreach (var i in ListIdInstrucctions)
                {
                    var Folio = int.Parse(i["N° Documento"].ToString());
                    try
                    {

                        var RutAcreedor = i["Rut"].ToString().Substring(0, 7);
                        var Glosa = i["Glosa"].ToString();
                        var montoBruto = 1;
                        try
                        {
                            montoBruto = int.Parse(i["Monto a pago"].ToString());
                        }
                        catch
                        {
                            montoBruto = int.Parse(i[" Monto a pago "].ToString());
                        }

                        var item = BDD.Where(e => e.Payment_matrix_natural_key == Glosa && e.Participants_creditor.Rut.Contains(RutAcreedor) && e.Amount_Gross == montoBruto).Select(item => item.ID).ToList()[0];
                        var bdPago = await _instruccionesDefRepository.GetByClienteIDAsync(item);
                        if (conditional == 0)
                        {
                            if (bdPago.Debtor != id)
                            {
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };
                            conditional = 1;
                        }
                        bdPago.Is_paid = true;
                        bdPago.Estado_pago = 2;
                        bdPago.Fecha_pago = FechaPago;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdPago))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {
                        numberList.Add(Folio);
                    }
                }

            }
            else if (bank == 9) // SECURITY
            {
                foreach (var i in ListIdInstrucctions)
                {
                    var Folio = int.Parse(i["DETALLE"].ToString());
                    try
                    {
                        var RutAcreedor = i["Rut"].ToString().Substring(0, 7);
                        var item = BDD.Where(e => e.Participants_creditor.Rut.Contains(RutAcreedor) && e.Folio == Folio && e.Debtor == id).Select(item => item.ID).ToList()[0];
                        var bdPago = await _instruccionesDefRepository.GetByClienteIDAsync(item);
                        if (conditional == 0)
                        {
                            if (bdPago.Debtor != id)
                            {
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };

                            conditional = 1;
                        }
                        bdPago.Is_paid = true;
                        bdPago.Estado_pago = 2;
                        bdPago.Fecha_pago = FechaPago;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdPago))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {
                        numberList.Add(Folio);
                    }
                }

            }
            else if (bank == 7) //   SANTANDER
            {

                foreach (var i in ListIdInstrucctions)
                {
                    var Folio = int.Parse(i["N Factura 1"].ToString());
                    try
                    {
                        var RutAcreedor = i["Rut Beneficiario"].ToString().Substring(0, 7);
                        var item = BDD.Where(e => e.Participants_creditor.Rut.Contains(RutAcreedor) && e.Folio == Folio && e.Debtor == id).Select(item => item.ID).ToList()[0];
                        var bdPago = await _instruccionesDefRepository.GetByClienteIDAsync(item);
                        if (conditional == 0)
                        {
                            if (bdPago.Debtor != id)
                            {
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };
                            conditional = 1;
                        }
                        bdPago.Is_paid = true;
                        bdPago.Estado_pago = 2;
                        bdPago.Fecha_pago = FechaPago;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdPago))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {
                        numberList.Add(Folio);
                    }
                }
            }
            else
            {
                return NotFound(new CodeErrorResponse(400, String.Concat("Este cliente no tiene Facturador")));
            }
            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);
                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "ERROR",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Nominas",
                    description = String.Concat("Se actualizo todo menos las instrucciones con folio ", lista),

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en ERROR"));
                }

                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con folio ", lista)));
            }
            else
            {

                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "OK",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Nominas",
                    description = "SE AGREGO CORRECTAMENTE",

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en OK"));
                }
                return Ok();
            }

        }

        [HttpPost("CuadreMasivoAcreedor")]
        public async Task<ActionResult> CuadreMasivoAcreedor(int id, string excelName, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            var Fecha_pago = DateTime.Now;
            List<int> numberList = new List<int>();
            var conditional = 0;
            foreach (var i in ListIdInstrucctions)
            {
                try
                {
                    var bd = await _instruccionesDefRepository.GetByClienteIDAsync(int.Parse(i["ID Instruccion"].ToString()));
                    if (conditional == 0)
                    {
                        if (bd.Creditor != id)
                        {
                            var clienteReal = _participantesRepository.GetByClienteIDAsync(id);
                            //var clienteInst = bd.Debtor;
                            return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece a ", clienteReal.Result.Business_Name)));
                        };
                        conditional = 1;
                    }
                    try
                    {
                        try
                        {
                            Fecha_pago = Convert.ToDateTime(i["Fecha de Pago"].ToString().Substring(0, 10));
                        }
                        catch
                        {
                            var fechita = i["Fecha de Pago"].ToString().Substring(0, 10);
                            Fecha_pago = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                        }
                    }
                    catch
                    {
                        Fecha_pago = DateTime.FromOADate(int.Parse(i["Fecha de Pago"].ToString()));
                    }
                    bd.Fecha_pago = Fecha_pago;
                    bd.Estado_pago = 2;
                    bd.Folio = int.Parse(i["Folio"].ToString());
                    bd.Is_paid = true;
                    if (!await _instruccionesDefRepository.UpdateeAsync(bd))
                    {
                        return StatusCode(500);
                    }
                }
                catch (Exception)
                {
                    numberList.Add(int.Parse(i["id_instruccion"].ToString()));
                }
            }


            ///
            ///FINAAAAAAL
            ///
            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);
                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "ERROR",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Acreedor",
                    description = String.Concat("Se actualizo todo menos las instrucciones con id ", lista),

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en ERROR"));
                }

                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con id ", lista)));
            }
            else
            {

                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "OK",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Acreedor",
                    description = "SE AGREGO CORRECTAMENTE",

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en OK"));
                }
                return Ok();
            }
        }
        [HttpPost("ActualizarFacDeudor")]
        public async Task<ActionResult> ActualizarFacDeudor(int id, string excelName, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            List<int> numberList = new List<int>();
            var conditional = 0;
            var BDDTrigonos = _context.Set<REACT_TRGNS_PROYECTOS>()
                .Where(e => e.vHabilitado == 1).Select(item => item.Id_participants).ToList();
            var FechaRecepcion = DateTime.UtcNow;
            var FechaHoy = DateTime.Today;
            var FechaPago = DateTime.UtcNow;
            var FechaEmision = DateTime.UtcNow;

            foreach (var i in ListIdInstrucctions)
            {
                try
                {
                    var bd = await _instruccionesDefRepository.GetByClienteIDAsync(int.Parse(i["ID Instruccion"].ToString()));
                    if (conditional == 0)
                    {
                        if (bd.Debtor != id)
                        {
                            var clienteReal = _participantesRepository.GetByClienteIDAsync(id);
                            //var clienteInst = bd.Debtor;
                            return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece a ", clienteReal.Result.Business_Name)));
                        };
                        conditional = 1;
                    }
                    try
                    {
                        try
                        {
                            FechaRecepcion = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10));
                        }
                        catch
                        {
                            var fechita = i["Fecha Recepcion"].ToString().Substring(0, 10);
                            FechaRecepcion = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                        }
                        //FechaEmisionDefontana = Convert.ToDateTime(Convert.ToDateTime(i["Fecha"].ToString().Substring(0, 10)).ToString("yyyy-MM-dd", new CultureInfo("ja-JP")));
                    }
                    catch
                    {
                        FechaRecepcion = DateTime.FromOADate(int.Parse(i["Fecha Recepcion"].ToString()));
                    };


                    //VERIFICAMOS FECHA DE RECEPCION
                    if (!(Convert.ToDateTime(FechaRecepcion.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(FechaHoy.ToString("yyyy-MM-dd"))))
                    {
                        throw new Exception("¡Esta es una excepción forzada!");
                    }
                    //


                    if (excelName.Contains("HIST"))
                    {
                        //
                        //TRY FECHA DE PAGO
                        //
                        try
                        {
                            try
                            {
                                FechaPago = Convert.ToDateTime(i["Fecha Pago"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["Fecha Pago"].ToString().Substring(0, 10);
                                FechaPago = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }                       
                        }
                        catch
                        {
                            FechaPago = DateTime.FromOADate(int.Parse(i["Fecha Pago"].ToString()));
                        };
                        //
                        //TRY FECHA DE EMISION
                        //
                        try
                        {
                            try
                            {
                                FechaEmision = Convert.ToDateTime(i["Fecha Emision"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["Fecha Emision"].ToString().Substring(0, 10);
                                FechaEmision = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }
                        }
                        catch
                        {
                            FechaEmision = DateTime.FromOADate(int.Parse(i["Fecha Emision"].ToString()));
                        };

                        //Console.WriteLine(FechaPago);
                        //Console.WriteLine(FechaRecepcion);
                        //Console.WriteLine(FechaEmision);
                        if(FechaPago < FechaEmision || FechaRecepcion < FechaEmision)
                        {
                            throw new Exception("¡Esta es una excepción forzada!");
                        }
                        ///
                        ///
                        ///
                        //ESTADOS
                        bd.Estado_emision = 2;
                        bd.Estado_recepcion = 1;
                        bd.Estado_aceptacion = 1;
                        bd.Estado_pago = 2;
                        bd.Is_paid = true;

                        //FECHAS
                        bd.Fecha_emision = FechaEmision;
                        bd.Fecha_aceptacion = FechaRecepcion;
                        bd.Fecha_recepcion = FechaRecepcion;
                        bd.Fecha_pago = FechaPago;
                        //DATOS
                        bd.Folio = int.Parse(i["Folio"].ToString());

                    }
                    else
                    {
                        if (!BDDTrigonos.Contains(int.Parse(bd.Creditor.ToString())))
                        {
                            //ESTADOS
                            bd.Estado_emision = 2;
                            //FECHAS
                            bd.Fecha_emision = FechaRecepcion;
                        }
                        //ESTADOS
                        bd.Estado_recepcion = 1;
                        bd.Estado_aceptacion = 1;
                        //FECHAS
                        bd.Fecha_aceptacion = FechaRecepcion;
                        bd.Fecha_recepcion = FechaRecepcion;
                        //DATOS
                        bd.Folio = int.Parse(i["Folio"].ToString());
                    }


                    if (!await _instruccionesDefRepository.UpdateeAsync(bd))
                    {
                        return StatusCode(500);
                    }
                }
                catch (Exception)
                {

                    numberList.Add(int.Parse(i["ID Instruccion"].ToString()));
                }
            }
            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);
                var mensaje = String.Concat("Se actualizaron " + (ListIdInstrucctions.Count - numberList.Count) + "/" + ListIdInstrucctions.Count +
                    " instrucciones, id de instrucciones con error :", lista);
                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "ERROR",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Deudor",
                    description = mensaje,

                };
                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en ERROR"));
                }
                return NotFound(new CodeErrorResponse(400, mensaje));
            }
            else
            {
                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "OK",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Deudor",
                    description = "SE AGREGO CORRECTAMENTE",

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en OK"));
                }
                return Ok();
            }
        }

        [HttpPost("FacturacionMasiva")]
        public async Task<ActionResult> FacturacionMasiva(int id, int erp, string excelName, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            var conditional = 0;
            //List<string> listado = new List<string>();
            string pruebaa = "NADA";
            var FechaEmisionAbastible = DateTime.UtcNow;
            List<int> numberList = new List<int>();
            var BDD = _context.Set<REACT_CEN_instructions_Def>()
                .Where(e => e.Folio == 0 && e.Amount > 9 && e.Creditor == id);
            if (erp == 1)
            {
                foreach (var i in ListIdInstrucctions)
                {

                    var montoNetoAbastible = int.Parse(i["neto"].ToString());
                    try
                    {
                        var glosaAbastible = i["RazonReferencia"].ToString();
                        var folioAbastible = int.Parse(i["Folio"].ToString());

                        try
                        {
                            try
                            {
                                FechaEmisionAbastible = Convert.ToDateTime(i["FechaEmision"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["FechaEmision"].ToString().Substring(0, 10);
                                FechaEmisionAbastible = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }
                            //FechaEmisionAbastible = Convert.ToDateTime(Convert.ToDateTime(i["FechaEmision"].ToString().Substring(0, 10)).ToString("yyyy-MM-dd", new CultureInfo("ja-JP")));
                        }
                        catch
                        {
                            FechaEmisionAbastible = DateTime.FromOADate(int.Parse(i["FechaEmision"].ToString()));
                        };
                        var rutAbastible = i["Rut"].ToString().Substring(0, 8);
                        var itemAbastible = BDD.Where(e => e.Creditor == id && e.Payment_matrix_natural_key == glosaAbastible && e.Amount == montoNetoAbastible && e.Participants_debtor.Rut.Contains(rutAbastible)).Select(item => item.ID).ToList()[0];
                        var bdAbastible = await _instruccionesDefRepository.GetByClienteIDAsync(itemAbastible);
                        if (conditional == 0)
                        {
                            if (bdAbastible.Creditor != id)
                            {
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };
                            conditional = 1;
                        }
                        bdAbastible.Estado_emision = 2;
                        bdAbastible.Folio = folioAbastible;
                        bdAbastible.Fecha_emision = FechaEmisionAbastible;
                        conditional = 1;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdAbastible))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {

                        numberList.Add(montoNetoAbastible);
                    }

                }
            }
            else if (erp == 7)
            {
                var FechaEmisionDefontana = DateTime.Now;
                foreach (var i in ListIdInstrucctions)
                {
                    var montoNetoDefontana = int.Parse(i["Afecto"].ToString());
                    try
                    {

                        var glosaDefontana = i["CodigodelProducto"].ToString();
                        var folioDefontana = int.Parse(i["NúmeroCorrelativo"].ToString());
                        try
                        {
                            try
                            {
                                FechaEmisionDefontana = Convert.ToDateTime(i["FechaEmision"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["FechaEmision"].ToString().Substring(0, 10);
                                FechaEmisionDefontana = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }
                            //FechaEmisionDefontana = Convert.ToDateTime(Convert.ToDateTime(i["Fecha"].ToString().Substring(0, 10)).ToString("yyyy-MM-dd", new CultureInfo("ja-JP")));
                        }
                        catch
                        {
                            FechaEmisionDefontana = DateTime.FromOADate(int.Parse(i["Fecha"].ToString()));
                        };

                        var rutDefontana = i["CódigodelCliente"].ToString().Substring(0, 8);
                        var itemDefontana = BDD/*.Where(e => e.Creditor == id)*/.Where(e => e.Payment_matrix_natural_key == glosaDefontana && e.Amount == montoNetoDefontana && e.Participants_debtor.Rut.Contains(rutDefontana)).Select(item => item.ID).ToList()[0];
                        var bdDefontana = await _instruccionesDefRepository.GetByClienteIDAsync(itemDefontana);
                        if (conditional == 0)
                        {
                            if (bdDefontana.Creditor != id)
                            {
                                //var clienteReal = _participantesRepository.GetByClienteIDAsync(id);
                                //var clienteInst = bd.Debtor;
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };
                            conditional = 1;
                        }
                        bdDefontana.Estado_emision = 2;
                        bdDefontana.Folio = folioDefontana;
                        bdDefontana.Fecha_emision = FechaEmisionDefontana;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdDefontana))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {

                        numberList.Add(montoNetoDefontana);
                    }

                }
            }
            else if (erp == 2)
            {
                var FechaEmisionNubox = DateTime.Now;
                foreach (var i in ListIdInstrucctions)
                {
                    var montoNetoNubox = int.Parse(i["Precio"].ToString());
                    try
                    {
                        var glosaNubox = i["Producto"].ToString();
                        try
                        {
                            try
                            {
                                FechaEmisionNubox = Convert.ToDateTime(i["Fecha Emision"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["Fecha Emision"].ToString().Substring(0, 10);
                                FechaEmisionNubox = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }

                        }
                        catch
                        {
                            FechaEmisionNubox = DateTime.FromOADate(int.Parse(i["Fecha Emision"].ToString()));
                        }
                        var rutNubox = i["Rut"].ToString().Substring(0, 8);
                        var folioNubox = int.Parse(i["FOLIO"].ToString());
                        var itemNubox = BDD.Where(e => e.Payment_matrix_natural_key == glosaNubox && e.Amount == montoNetoNubox && e.Participants_debtor.Rut.Contains(rutNubox)).Select(item => item.ID).ToList()[0];
                        var bdNubox = await _instruccionesDefRepository.GetByClienteIDAsync(itemNubox);
                        if (conditional == 0)
                        {
                            if (bdNubox.Creditor != id)
                            {
                                return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece al cliente seleccionado")));
                            };
                            conditional = 1;
                        }
                        bdNubox.Estado_emision = 2;
                        bdNubox.Folio = folioNubox;
                        bdNubox.Fecha_emision = FechaEmisionNubox;
                        if (!await _instruccionesDefRepository.UpdateeAsync(bdNubox))
                        {
                            return StatusCode(500);
                        }
                    }
                    catch (Exception)
                    {
                        numberList.Add(montoNetoNubox);
                    }

                }

            }
            else if (conditional == 0)
            {
                return NotFound(new CodeErrorResponse(400, String.Concat("El cliente no tiene Facturador o Seleccione bien al cliente ")));
            }
            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);


                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "ERROR",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Facturacion Masiva",
                    description = String.Concat("Se actualizo todo menos las instrucciones con montoNeto ", lista),

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en ERROR"));
                }
                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con montoNeto ", lista)));
            }
            else
            {
                var excel = new REACT_TRGNS_Excel_History
                {
                    excelName = excelName,
                    status = "OK",
                    date = fechaHoraActualChile,
                    idParticipant = id,
                    type = "Facturacion Masiva",
                    description = "SE AGREGO CORRECTAMENTE",

                };

                if (!await _excelHistoryRepository.SaveBD(excel))
                {
                    return BadRequest(new CodeErrorResponse(500, "Error al subir el excel en OK"));
                }
                return Ok();

            }

        }

        [HttpPost("FacturacionCL")]
        public async Task<ActionResult> FacturacionCL(int id, List<int> ListIdInstrucctions)
        {
            string RemoveAccents(string input)
            {
                string normalizedString = input.Normalize(NormalizationForm.FormD);
                StringBuilder stringBuilder = new StringBuilder();

                foreach (char c in normalizedString)
                {
                    UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder.ToString();
            }
            var FechaHoy = DateTime.UtcNow;
            //ASINANDO DATOS FACTURACION 
            var datosFacturacion = _context.Set<REACT_TRGNS_FACTCLDATA>()
            .Where(e => e.IdParticipante == id)
            .ToList();
            //TEST
            var UsuarioTest = DecodeToString(datosFacturacion[0].UsuarioTest);
            var ClaveTest = DecodeToString(datosFacturacion[0].ClaveTest);
            var RutTest = DecodeToString(datosFacturacion[0].RutTest);
            //PRODUCCION
            var UsuarioProduccion = DecodeToString(datosFacturacion[0].Usuario64);
            var ClaveProduccion = DecodeToString(datosFacturacion[0].Clave64);
            var RutProduccion = DecodeToString(datosFacturacion[0].RUT64);
            foreach (var i in ListIdInstrucctions)
            {

                //var encodeList = await _factClRepository.GetAllAsync();
                //var busqueda = encodeList.FirstOrDefault(i => i.IdParticipante == id);

                //if (busqueda == null)
                //{
                //    return NotFound(new CodeErrorResponse(404, "la facturacion no existe"));
                //}
                //else
                //{
                //    return new FacturacionClDto
                //    {
                //        ID = busqueda.ID,
                //        IdParticipante = busqueda.IdParticipante,
                //        Usuario64 = busqueda.Usuario64 != null ? DecodeToString(busqueda.Usuario64) : null,
                //        RUT64 = busqueda.RUT64 != null ? DecodeToString(busqueda.RUT64) : null,
                //        Clave64 = busqueda.Clave64 != null ? DecodeToString(busqueda.Clave64) : null,
                //        Puerto64 = busqueda.Puerto64 != null ? DecodeToString(busqueda.Puerto64) : null,
                //        IncluyeLink64 = busqueda.IncluyeLink64 != null ? DecodeToString(busqueda.IncluyeLink64) : null,
                //        UsuarioTest = busqueda.UsuarioTest != null ? DecodeToString(busqueda.UsuarioTest) : null,
                //        ClaveTest = busqueda.ClaveTest != null ? DecodeToString(busqueda.ClaveTest) : null,
                //        RutTest = busqueda.RutTest != null ? DecodeToString(busqueda.RutTest) : null,
                //        Phabilitado = busqueda.Phabilitado,
                //    };
                //}
                //DATOS INSTRUCCION

                var instrucción = _context.Set<REACT_CEN_instructions_Def>()
                .Where(e => e.ID == i)
                .Include(p => p.CEN_dte_acceptance_status)
                .Include(p => p.TRGNS_dte_reception_status)
                .Include(p => p.CEN_payment_status_type)
                .Include(p => p.CEN_billing_status_type)
                .Include(p => p.cEN_Payment_Matrices.CEN_billing_windows)
                .Include(p => p.cEN_Payment_Matrices)
                .Include(p => p.Participants_creditor)
                .Include(p => p.Participants_debtor)
                .ToList();




                var folio = instrucción[0].Folio;
                var montoNeto = instrucción[0].Amount;
                var montoBruto = instrucción[0].Amount_Gross;
                var concepto = instrucción[0].Payment_matrix_natural_key;
                var codigoReferencia = instrucción[0].cEN_Payment_Matrices.Reference_code;
                var fechaCarta = instrucción[0].cEN_Payment_Matrices.Publish_date;

                //DATOS ACREEDOR
                var rutAcreedor = instrucción[0].Participants_creditor.Rut + '-' + instrucción[0].Participants_creditor.Verification_Code;
                var nombreComercialAcreedor = instrucción[0].Participants_creditor.Business_Name;
                var giroAcreedor = instrucción[0].Participants_creditor.Commercial_Business;
                var direccionComercialAcreedor = instrucción[0].Participants_creditor.Commercial_address;
                //DATOS DEUDOR
                var rutDeudor = instrucción[0].Participants_debtor.Rut + '-' + instrucción[0].Participants_creditor.Verification_Code;
                var nombreComercialDeudor = instrucción[0].Participants_debtor.Business_Name;
                var giroDeudor = instrucción[0].Participants_debtor.Commercial_Business;
                var direccionComercialDeudor = instrucción[0].Participants_debtor.Commercial_address;
                //DATA DE ENCABEZADO
                Dictionary<string, object> item = new Dictionary<string, object>();
                item.Add("tipoDTE", 33);
                item.Add("foliod", folio);
                item.Add("fechaEmision", DateTime.Today.ToString("yyyyy-MM-dd"));
                item.Add("tpoTranVenta", 1);
                item.Add("fmaPago", 2);
                //EMISOR
                item.Add("rutacreedor", rutAcreedor);
                item.Add("rznSoc", RemoveAccents(nombreComercialAcreedor));
                item.Add("GiroEmis", RemoveAccents(giroAcreedor));
                item.Add("CorreoEmisor", RemoveAccents("hvits@pelicanosolar.cl"));
                item.Add("Acteco", 351019);
                item.Add("dirOrigen", RemoveAccents(direccionComercialAcreedor));
                item.Add("CmunaOrigen", "Las Condes");
                item.Add("CiudadOrigen", "Santiago");
                //RECEPTOR
                item.Add("rutDebtor", rutDeudor);
                item.Add("RznSocRecep", RemoveAccents(nombreComercialDeudor));
                item.Add("GiroRecep", RemoveAccents(giroDeudor));
                item.Add("dirDestino", RemoveAccents(direccionComercialDeudor));
                item.Add("CmunaDestino", "Santiago");
                item.Add("ciudadreceptor", "Santiago");
                //TOTALES
                item.Add("MntoNeto", montoNeto);
                item.Add("MntoExento", 0);
                item.Add("tazaIva", 19);
                item.Add("IVA", "");
                item.Add("MntoBruto", montoBruto);
                //DETALLE
                item.Add("NroLinDet", 1);
                item.Add("tipoCodigo", "INT1");
                item.Add("VlrCodigo", 0);
                item.Add("NmbItem", RemoveAccents(concepto));
                item.Add("QtyItem", "1");
                item.Add("UnmdItem", "UN");
                item.Add("PrcItem", montoNeto);
                item.Add("MontoItem", montoNeto);
                //REFERENCIAS
                item.Add("NroLinRef", 1);
                item.Add("TpoDocRef", 802);
                item.Add("FolioReferencia", codigoReferencia);
                item.Add("FechaRef", fechaCarta);
                item.Add("RazonDIF", RemoveAccents(concepto));


                XmlDocument doc = new XmlDocument();
                //Creacion XML
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement ElementoDTE = doc.CreateElement(string.Empty, "DTE", string.Empty);
                doc.AppendChild(ElementoDTE);

                XmlElement ElementoDocumento = doc.CreateElement(string.Empty, "Documento", string.Empty);
                ElementoDTE.AppendChild(ElementoDocumento);

                XmlElement elementodteV =
                    doc.CreateElement(string.Empty, "Encabezado", string.Empty);
                ElementoDocumento.AppendChild(elementodteV);

                XmlElement element2 = doc.CreateElement(string.Empty, "IdDoc", string.Empty);
                elementodteV.AppendChild(element2);

                XmlElement elementTipoDTE = doc.CreateElement(string.Empty, "TipoDTE", string.Empty);
                XmlText tipoDTE = doc.CreateTextNode(Convert.ToString(item["tipoDTE"]));
                elementTipoDTE.AppendChild(tipoDTE);
                element2.AppendChild(elementTipoDTE);

                XmlElement elementFolio = doc.CreateElement(string.Empty, "Folio", string.Empty);
                XmlText Folio = doc.CreateTextNode(Convert.ToString(item["foliod"]));
                elementFolio.AppendChild(Folio);
                element2.AppendChild(elementFolio);

                XmlElement elementFchEmis = doc.CreateElement(string.Empty, "FchEmis", string.Empty);
                XmlText FechaEmision = doc.CreateTextNode(Convert.ToString(item["fechaEmision"]));
                elementFchEmis.AppendChild(FechaEmision);
                element2.AppendChild(elementFchEmis);

                XmlElement elementTpoTransVenta = doc.CreateElement(string.Empty, "TpoTranVenta", string.Empty);
                XmlText TpoTransVenta = doc.CreateTextNode(Convert.ToString(item["tpoTranVenta"]));
                elementTpoTransVenta.AppendChild(TpoTransVenta);
                element2.AppendChild(elementTpoTransVenta);

                XmlElement elementTermPagoGlosa = doc.CreateElement(string.Empty, "TermPagoGlosa", string.Empty);
                XmlText FmaPago = doc.CreateTextNode(Convert.ToString(item["fmaPago"]));
                elementTermPagoGlosa.AppendChild(FmaPago);
                element2.AppendChild(elementTermPagoGlosa);

                //EMISOR
                XmlElement elementEmisor = doc.CreateElement(string.Empty, "Emisor", string.Empty);
                elementodteV.AppendChild(elementEmisor);

                XmlElement elementRutEmisor = doc.CreateElement(string.Empty, "RUTEmisor", string.Empty);
                XmlText RUTEmisor = doc.CreateTextNode(Convert.ToString(item["rutacreedor"]));
                elementRutEmisor.AppendChild(RUTEmisor);
                elementEmisor.AppendChild(elementRutEmisor);

                XmlElement elementRznSocial = doc.CreateElement(string.Empty, "RznSoc", string.Empty);
                XmlText RznSoc = doc.CreateTextNode(Convert.ToString(item["rznSoc"]));
                elementRznSocial.AppendChild(RznSoc);
                elementEmisor.AppendChild(elementRznSocial);

                XmlElement elementGiro = doc.CreateElement(string.Empty, "GiroEmis", string.Empty);
                XmlText GiroEmis = doc.CreateTextNode(Convert.ToString(item["GiroEmis"]));
                elementGiro.AppendChild(GiroEmis);
                elementEmisor.AppendChild(elementGiro);

                XmlElement elementCorreoEmisor = doc.CreateElement(string.Empty, "CorreoEmisor", string.Empty);
                XmlText CorreoEmisor = doc.CreateTextNode(Convert.ToString(item["CorreoEmisor"]));
                elementCorreoEmisor.AppendChild(CorreoEmisor);
                elementEmisor.AppendChild(elementCorreoEmisor);


                XmlElement elementActeco = doc.CreateElement(string.Empty, "Acteco", string.Empty);
                XmlText Acteco = doc.CreateTextNode(Convert.ToString(item["Acteco"]));
                elementActeco.AppendChild(Acteco);
                elementEmisor.AppendChild(elementActeco);


                XmlElement elementDirOrigen = doc.CreateElement(string.Empty, "DirOrigen", string.Empty);
                XmlText DirOrigen = doc.CreateTextNode(Convert.ToString(item["dirOrigen"]));
                elementDirOrigen.AppendChild(DirOrigen);
                elementEmisor.AppendChild(elementDirOrigen);

                XmlElement elementCmnaOrigen = doc.CreateElement(string.Empty, "CmnaOrigen", string.Empty);
                XmlText CmnaOrigen = doc.CreateTextNode(Convert.ToString(item["CmunaOrigen"]));
                elementCmnaOrigen.AppendChild(CmnaOrigen);
                elementEmisor.AppendChild(elementCmnaOrigen);

                XmlElement elementCiudadOrigen = doc.CreateElement(string.Empty, "CiudadOrigen", string.Empty);
                XmlText CiudadOrigen = doc.CreateTextNode(Convert.ToString(item["CiudadOrigen"]));
                elementCiudadOrigen.AppendChild(CiudadOrigen);
                elementEmisor.AppendChild(elementCiudadOrigen);

                //Receptor
                XmlElement elementReceptor = doc.CreateElement(string.Empty, "Receptor", string.Empty);
                elementodteV.AppendChild(elementReceptor);

                XmlElement element15 = doc.CreateElement(string.Empty, "RUTRecep", string.Empty);
                XmlText RUTRecep = doc.CreateTextNode(Convert.ToString(item["rutDebtor"]));
                element15.AppendChild(RUTRecep);
                elementReceptor.AppendChild(element15);

                XmlElement element16 = doc.CreateElement(string.Empty, "RznSocRecep", string.Empty);
                XmlText RznSocRecep = doc.CreateTextNode(Convert.ToString(item["RznSocRecep"]));
                element16.AppendChild(RznSocRecep);
                elementReceptor.AppendChild(element16);

                XmlElement element17 = doc.CreateElement(string.Empty, "GiroRecep", string.Empty);
                XmlText GiroRecep = doc.CreateTextNode(Convert.ToString(item["GiroRecep"]));
                element17.AppendChild(GiroRecep);
                elementReceptor.AppendChild(element17);

                XmlElement element18 = doc.CreateElement(string.Empty, "DirRecep", string.Empty);
                XmlText DirRecep = doc.CreateTextNode(Convert.ToString(item["dirDestino"]));
                element18.AppendChild(DirRecep);
                elementReceptor.AppendChild(element18);

                XmlElement element19 = doc.CreateElement(string.Empty, "CmnaRecep", string.Empty);
                XmlText CmnaRecep = doc.CreateTextNode(Convert.ToString(item["CmunaDestino"]));
                element19.AppendChild(CmnaRecep);
                elementReceptor.AppendChild(element19);

                XmlElement elementRecep = doc.CreateElement(string.Empty, "CiudadRecep", string.Empty);
                XmlText CiudadRecep = doc.CreateTextNode(Convert.ToString(item["ciudadreceptor"]));
                elementRecep.AppendChild(CiudadRecep);
                elementReceptor.AppendChild(elementRecep);

                XmlElement elementTotales = doc.CreateElement(string.Empty, "Totales", string.Empty);
                elementodteV.AppendChild(elementTotales);

                XmlElement elementMntNeto = doc.CreateElement(string.Empty, "MntNeto", string.Empty);
                XmlText MntNeto = doc.CreateTextNode(Convert.ToString(item["MntoNeto"]));
                elementMntNeto.AppendChild(MntNeto);
                elementTotales.AppendChild(elementMntNeto);

                XmlElement elementMntExe = doc.CreateElement(string.Empty, "MntExe", string.Empty);
                XmlText MntExe = doc.CreateTextNode(Convert.ToString(item["MntoExento"]));
                elementMntExe.AppendChild(MntExe);
                elementTotales.AppendChild(elementMntExe);

                XmlElement element22 = doc.CreateElement(string.Empty, "TasaIVA", string.Empty);
                XmlText TasaIVA = doc.CreateTextNode(Convert.ToString(item["tazaIva"]));
                element22.AppendChild(TasaIVA);
                elementTotales.AppendChild(element22);

                //Calculo de IVA
                var MontoNetov2 = item["MntoNeto"];
                var ivav2 = Math.Round(Convert.ToInt32(MontoNetov2) * 0.19);

                XmlElement element23 = doc.CreateElement(string.Empty, "IVA", string.Empty);
                XmlText IVA = doc.CreateTextNode(Convert.ToString(ivav2));
                element23.AppendChild(IVA);
                elementTotales.AppendChild(element23);

                XmlElement element24 = doc.CreateElement(string.Empty, "MntTotal", string.Empty);
                XmlText MntTotal = doc.CreateTextNode(Convert.ToString(item["MntoBruto"]));
                element24.AppendChild(MntTotal);
                elementTotales.AppendChild(element24);

                //Detalle Fuera de Encabezado
                XmlElement elementoDetalle =
                doc.CreateElement(string.Empty, "Detalle", string.Empty);
                ElementoDocumento.AppendChild(elementoDetalle);

                XmlElement element25 = doc.CreateElement(string.Empty, "NroLinDet", string.Empty);
                XmlText NroLinDet = doc.CreateTextNode(Convert.ToString(item["NroLinDet"]));
                element25.AppendChild(NroLinDet);
                elementoDetalle.AppendChild(element25);

                //SubElemento
                XmlElement elementoCodItem = doc.CreateElement(string.Empty, "CdgItem", string.Empty);
                elementoDetalle.AppendChild(elementoCodItem);

                XmlElement elementTpoCodigo = doc.CreateElement(string.Empty, "TpoCodigo", string.Empty);
                XmlText TpoCodigo = doc.CreateTextNode(Convert.ToString(item["tipoCodigo"]));
                elementTpoCodigo.AppendChild(TpoCodigo);
                elementoCodItem.AppendChild(elementTpoCodigo);

                XmlElement elementVlrCodigo = doc.CreateElement(string.Empty, "VlrCodigo", string.Empty);
                XmlText VlrCodigo = doc.CreateTextNode(Convert.ToString(item["VlrCodigo"]));
                elementVlrCodigo.AppendChild(VlrCodigo);
                elementoCodItem.AppendChild(elementVlrCodigo);
                //FIN SUB Elemento

                XmlElement element26 = doc.CreateElement(string.Empty, "NmbItem", string.Empty);
                XmlText NmbItem = doc.CreateTextNode(Convert.ToString(item["NmbItem"]));
                element26.AppendChild(NmbItem);
                elementoDetalle.AppendChild(element26);

                XmlElement element27d = doc.CreateElement(string.Empty, "QtyItem", string.Empty);
                XmlText QtyItem2 = doc.CreateTextNode(Convert.ToString(item["QtyItem"]));
                element27d.AppendChild(QtyItem2);
                elementoDetalle.AppendChild(element27d);

                XmlElement elementUnmdItem = doc.CreateElement(string.Empty, "UnmdItem", string.Empty);
                XmlText UnmdItem = doc.CreateTextNode(Convert.ToString(item["UnmdItem"]));
                elementUnmdItem.AppendChild(UnmdItem);
                elementoDetalle.AppendChild(elementUnmdItem);

                XmlElement element28 = doc.CreateElement(string.Empty, "PrcItem", string.Empty);
                XmlText PrcItem = doc.CreateTextNode(Convert.ToString(item["MontoItem"]));
                element28.AppendChild(PrcItem);
                elementoDetalle.AppendChild(element28);

                XmlElement element29 = doc.CreateElement(string.Empty, "MontoItem", string.Empty);
                XmlText MontoItem = doc.CreateTextNode(Convert.ToString(item["MontoItem"]));
                element29.AppendChild(MontoItem);
                elementoDetalle.AppendChild(element29);

                //Referencia
                XmlElement elementoReferencia = doc.CreateElement(string.Empty, "Referencia", string.Empty);
                ElementoDocumento.AppendChild(elementoReferencia);

                XmlElement element30 = doc.CreateElement(string.Empty, "NroLinRef", string.Empty);
                XmlText NroLinRef = doc.CreateTextNode(Convert.ToString(item["NroLinRef"]));
                element30.AppendChild(NroLinRef);
                elementoReferencia.AppendChild(element30);

                XmlElement element31 = doc.CreateElement(string.Empty, "TpoDocRef", string.Empty);
                XmlText TpoDocRef = doc.CreateTextNode(Convert.ToString(item["TpoDocRef"]));
                element31.AppendChild(TpoDocRef);
                elementoReferencia.AppendChild(element31);


                XmlElement element32 = doc.CreateElement(string.Empty, "FolioRef", string.Empty);
                XmlText FolioRef = doc.CreateTextNode(Convert.ToString(item["FolioReferencia"]));
                element32.AppendChild(FolioRef);
                elementoReferencia.AppendChild(element32);

                XmlElement element33 = doc.CreateElement(string.Empty, "FchRef", string.Empty);
                XmlText FchRef = doc.CreateTextNode(Convert.ToString(item["FechaRef"]));
                element33.AppendChild(FchRef);
                elementoReferencia.AppendChild(element33);

                XmlElement element34 = doc.CreateElement(string.Empty, "RazonRef", string.Empty);
                XmlText RazonRef = doc.CreateTextNode(Convert.ToString(item["RazonDIF"]));
                element34.AppendChild(RazonRef);
                elementoReferencia.AppendChild(element34);
                string filePath = "C:/home/site/wwwroot/FacturacionXml/" + item["rutacreedor"] + "/" + item["rutacreedor"].ToString() + "_" + id.ToString() + "_" + item["foliod"].ToString() + "_" + instrucción[0].ID.ToString() + ".xml";
                //string filePath = "C:/Users/neidr/Desktop/TRGNS/file.xml";
                //string filePath = "C:/home/site/wwwroot/FacturacionXml/file.xml";

                //File.WriteAllBytes(@"E:\Folder\"+ fileName, Convert.FromBase64String(Base64String));
                doc.Save(filePath);

                byte[] arrayDeBytes = System.IO.File.ReadAllBytes(filePath);

                string codificado = Convert.ToBase64String(arrayDeBytes);

                //Enviar al Facturador.CL

                wsplanoSoapClient client = new wsplanoSoapClient();
                logininfo login = new logininfo();


                login.Usuario = UsuarioTest; //"UEVMSUNBTk8=";

                login.Rut = RutTest; //"NzYzMzc1OTktNA=="; //Testing: MS05 //Produccion:NzYzMzc1OTktNA==

                login.Clave = ClaveTest;//"ODRjZTEyNDRhMA=="; //Testing: cGxhbm85MTA5OA== // Produccion:ODRjZTEyNDRhMA==

                login.Puerto = "MQ==";

                login.IncluyeLink = "";

                Facturacion.cl.ProcesarRequest request = new ProcesarRequest();

                request.login = login;

                request.file = codificado;

                request.formato = 2;

                ProcesarResponse response = client.Procesar(request);



                ////Almacenar Respuestas

                ////try catch

                //try

                //{

                //    //Asignar Variables

                //    string _xml = response.ProcesarResult;

                //    XElement xdocumento = XElement.Parse(_xml);

                //    var elementsResult = xdocumento.Elements("Resultado");

                //    var responseOK = elementsResult.ToList()[0].Value;



                //    if (responseOK == "True")

                //    {

                //        var s1 = xdocumento.Elements("Detalle").Elements("Documento").Elements("Folio");



                //        var list = from items in xdocumento.Elements("Detalle").Elements("Documento")

                //                   select new

                //                   {

                //                       Folioone = items.Value



                //                   };



                //        var FolioFInal = s1.ToList()[0].Value;



                //        //Guardado en BD

                //        string query = "UPDATE dbo.BTAM_instrucciones_acreedor set folio=" + FolioFInal + ", emission_dt =" + "'" + Convert.ToDateTime(item.fechaEmision).ToString("dd-MM-yyyy") + "'" + ",status_billed=" + 2 + " where id_instruccion = " + id + "";



                //        db.ExecuteCommand(query);



                //        Response = "Exito";

                //    }

                //    else

                //    {

                //        elementsResult = xdocumento.Elements("Detalle").Elements("Documento").Elements("Error");

                //        var responseError = elementsResult.ToList()[0].Value + " " + "Cliente: " + lista[0].RznSocRecep;

                //        return Response = responseError;

                //    }



                //}

                //catch (Exception ex)

                //{

                //    Response = "ERROR" + response.ToString();

                //    throw;

                //}

            }




            return Ok();

        }
        //[HttpPost("Agregar")]
        //public async Task<IActionResult> AgregarExcel([FromBody] agregarExcelDto agregarExcel)
        //{

        //}
        [HttpPost("ActualizarEstEmision")]
        public async Task<ActionResult> ActualizarEstEmision(List<int?> ListIdInstrucctions, int estadoEmision)
        {
            var entityToUpdate = await _instruccionesDefRepository.GetAllAsync();

            for (var i = 0; i < estadoEmision; i++)
            {



            }
            var filteredList = entityToUpdate.Where(item => ListIdInstrucctions.Any(id => id == item.ID)).ToList().Select(item =>
            {
                item.Estado_emision = estadoEmision;
                return item;
            }).ToList();




            if (!await _instruccionesDefRepository.UpdateRangeBD(filteredList))
            {
                return BadRequest(new CodeErrorResponse(500, "No se logro realizar la operación"));
            }
            else
            {
                return Ok();
            }
        }

    }
}
