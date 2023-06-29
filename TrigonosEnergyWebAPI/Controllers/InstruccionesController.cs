﻿using AutoMapper;
using AutoMapper.Execution;
using Core.Entities;

using Core.Interface;
using Core.Specifications;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using LogicaTrigonos.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

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

        private readonly IGenericRepository<REACT_TRGNS_Excel_History> _excelHistoryRepository;
        //private readonly IGenericRepository<Patch_TRGNS_Datos_Facturacion> _instruccionessRepository;
        private readonly IMapper _mapper;
        private readonly TrigonosDBContext _context;
        public InstruccionesController(IGenericRepository<REACT_CEN_Participants> participantesRepository, TrigonosDBContext context, IGenericRepository<REACT_TRGNS_Excel_History> excelHistoryRepository, IGenericRepository<REACT_CEN_instructions_Def> instruccionesDefRepository, IGenericRepository<REACT_TRGNS_H_Datos_Facturacion> historificacionInstruccionesRepository, IGenericRepository<REACT_TRGNS_Datos_Facturacion> instruccionesRepository/*, IGenericRepository<Patch_TRGNS_Datos_Facturacion> instruccionessRepository*/, IMapper mapper, IGenericRepository<REACT_CEN_payment_matrices> matricesRepository)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
            _matricesRepository = matricesRepository;
            _historificacionInstruccionesRepository = historificacionInstruccionesRepository;
            _instruccionesDefRepository = instruccionesDefRepository;
            _excelHistoryRepository = excelHistoryRepository;
            _context = context;
            _participantesRepository = participantesRepository;
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarExcel([FromBody] agregarExcelDto agregarExcel)
        {
            var excel = new REACT_TRGNS_Excel_History
            {
                excelName = agregarExcel.excelName,
                status = agregarExcel.status,
                date = DateTime.Now,
                idParticipant = agregarExcel.idParticipant,
                type = agregarExcel.type,
                description = agregarExcel.description,

            };

            if (!await _excelHistoryRepository.SaveBD(excel))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha agregado la empresa"));
            }
            return Ok();
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

        [HttpPost("CuadreMasivoAcreedor")]
        public async Task<ActionResult> CuadreMasivoAcreedor(int id, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            var Fecha_pago = DateTime.Now;
            List<int> numberList = new List<int>();
            var conditional = 0;
            foreach (var i in ListIdInstrucctions)
            {
                try
                {
                    Console.WriteLine(int.Parse(i["ID Instruccion"].ToString()));
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
            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);

                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con id ", lista)));
            }
            return Ok();
        }
        [HttpPost("ActualizarFacDeudor")]
        public async Task<ActionResult> ActualizarFacDeudor(int id, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            //CultureInfo europeanCulture = new CultureInfo("en-GB");
            List<int> numberList = new List<int>();
            //List<int> idTrigonosList = new List<int>();
            var conditional = 0;
            var BDDTrigonos = _context.Set<REACT_TRGNS_PROYECTOS>()
                .Where(e => e.vHabilitado == 1).Select(item => item.Id_participants).ToList();
            try
            {
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
                        if (BDDTrigonos.Contains(int.Parse(bd.Creditor.ToString())))
                        {
                            bd.Estado_emision = 2;
                            bd.Estado_recepcion = 1;
                            bd.Estado_aceptacion = 1;
                            bd.Fecha_aceptacion = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10));
                            bd.Fecha_emision = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10));
                            bd.Fecha_recepcion = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10));
                            bd.Folio = int.Parse(i["Folio"].ToString());
                        }
                        else
                        {
                            bd.Estado_emision = 2;
                            bd.Estado_recepcion = 1;
                            bd.Estado_aceptacion = 1;
                            bd.Fecha_aceptacion = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10)); ;
                            bd.Fecha_recepcion = Convert.ToDateTime(i["Fecha Recepcion"].ToString().Substring(0, 10)); ;
                            bd.Folio = int.Parse(i["Folio"].ToString()); ;
                        }
                        //conditional = 1;
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
            }
            catch (Exception)
            {

            }

            if (numberList.Count > 0)
            {
                string lista = String.Join(",", numberList);

                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con id ", lista)));
            }
            return Ok();
        }

        [HttpPost("FacturacionMasiva")]
        public async Task<ActionResult> FacturacionMasiva(int id, int erp, List<Dictionary<string, object>> ListIdInstrucctions)
        {
            var conditional = 0;
            //List<string> listado = new List<string>();
            string pruebaa = "NADA";
            List<int> numberList = new List<int>();
            var BDD = _context.Set<REACT_CEN_instructions_Def>()
                .Where(e => e.Folio == 0 && e.Amount > 9 && e.Creditor == id);
            if (erp == 1)
            {
                foreach (var i in ListIdInstrucctions)
                {
                    var FechaEmisionAbastible = DateTime.UtcNow;
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

                                //var clienteReal = _participantesRepository.GetByClienteIDAsync(id);
                                //var clienteInst = bd.Debtor;
                                //return NotFound(new CodeErrorResponse(400, String.Concat("El excel subido no pertenece a ", clienteReal.Result.Business_Name)));
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
                    pruebaa = "entro 1";
                    var montoNetoNubox = int.Parse(i["Precio"].ToString());
                    try
                    {
                        pruebaa = "entro 2";
                        var glosaNubox = i["Producto"].ToString();
                        pruebaa = "entro 3";
                        var folioNubox = int.Parse(i["FOLIO"].ToString());
                        try
                        {
                            //var fechita = i["Fecha Emision"].ToString().Substring(0, 10);
                            //Console.WriteLine(Convert.ToDateTime(i["Fecha Emision"].ToString().Substring(0, 10)).ToString("yyyy-MM-dd", new CultureInfo("ja-JP")));
                            //Console.WriteLine(Convert.ToDateTime(Convert.ToDateTime(i["Fecha Emision"].ToString().Substring(0, 10)).ToString("yyyy-MM-dd", new CultureInfo("ja-JP"))));
                            //Console.WriteLine();09-02-2929
                            //DateTime fecha = DateTime.ParseExact(i["Fecha Emision"].ToString().Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            try
                            {
                                FechaEmisionNubox = Convert.ToDateTime(i["FechaEmision"].ToString().Substring(0, 10));
                            }
                            catch
                            {
                                var fechita = i["FechaEmision"].ToString().Substring(0, 10);
                                FechaEmisionNubox = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));
                            }
                            pruebaa = "entro 4";

                            //FechaEmisionNubox = Convert.ToDateTime(fechita.Substring(6, 4) + "-" + fechita.Substring(3, 2) + "-" + fechita.Substring(0, 2));

                        }
                        catch
                        {
                            pruebaa = "entro 5";
                            FechaEmisionNubox = DateTime.FromOADate(int.Parse(i["Fecha Emision"].ToString()));
                        }
                        pruebaa = "entro 6";
                        var rutNubox = i["Rut"].ToString().Substring(0, 8);
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
                return NotFound(new CodeErrorResponse(400, String.Concat("Se actualizo todo menos las instrucciones con montoNeto ", pruebaa)));
            }
            return Ok();
        }
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
