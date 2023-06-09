using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIMetrics")]

    public class MetricsController : BaseApiController
    {
        private readonly IGenericRepository<REACT_CEN_instructions_Def> _instruccionesDefRepository;
        private readonly IMapper _mapper;
        public MetricsController( IGenericRepository<REACT_CEN_instructions_Def> instruccionesDefRepository , IMapper mapper)
        {
            _mapper = mapper;
            _instruccionesDefRepository = instruccionesDefRepository;
    
        }
        [HttpGet("EstadoDePago/{id}")]
        public async Task<ActionResult<MetricEstadoPagoDto>> MetricaDePago(int id, bool? is_creditor)
        {

            var cant_total = new InstruccionesMetrics(id, null, is_creditor);
            var cant_no_pagado = new InstruccionesMetrics(id, 1, is_creditor);
            var cant_pagado = new InstruccionesMetrics(id, 2, is_creditor);
            var cant_pago_atrasado = new InstruccionesMetrics(id, 3, is_creditor);

            var t_pagado = await _instruccionesDefRepository.CountAsync(cant_pagado);
            var t_no_pagado = await _instruccionesDefRepository.CountAsync(cant_no_pagado);
            var t_atrasado = await _instruccionesDefRepository.CountAsync(cant_pago_atrasado);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(cant_total);
            return new MetricEstadoPagoDto
            {
                IDParticipante =  id,
                TotalNoPagado = t_no_pagado,
                TotalPagado = t_pagado,
                TotalAtrasado = t_atrasado,
                TotalMuestra = totalinstrucciones,
            };



        }
        [HttpGet("EstadoDeRecepcion/{id}")]
        public async Task<ActionResult<MetricsEstadoRecepciontDto>> MetricadeRecepcion(int id, bool? is_creditor)
        {

            var cant_total = new InstruccionesMetrics(id, is_creditor, null);
            var cant_recepcionado = new InstruccionesMetrics(id,is_creditor,1);
            var cant_norecepcionado = new InstruccionesMetrics(id, is_creditor,2);
            var cant_rechazado = new InstruccionesMetrics(id, is_creditor,3);


            var t_recept = await _instruccionesDefRepository.CountAsync(cant_recepcionado);
            var t_no_recept = await _instruccionesDefRepository.CountAsync(cant_norecepcionado);
            var t_rechazado = await _instruccionesDefRepository.CountAsync(cant_rechazado);

            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(cant_total);
            return new MetricsEstadoRecepciontDto
            {
                IDParticipante = id,
                TotalRecepcionado = t_recept,
                TotalNoRecepcionado = t_no_recept,
                TotalRechazado = t_rechazado,
                TotalMuestra = totalinstrucciones,
            };



        }
        [HttpGet("EstadoDeFacturacion/{id}")]
        public async Task<ActionResult<MetricsEstadoFacturadoDto>> MetricadeFacturacion(int id, bool? is_creditor)
        {

            var cant_total = new InstruccionesMetrics(is_creditor, id, null);
            var cant_NoFacturado = new InstruccionesMetrics(is_creditor, id, 1);
            var cant_Facturado = new InstruccionesMetrics(is_creditor, id, 2);
            var cant_FacturadoAtraso = new InstruccionesMetrics(is_creditor, id, 3);
            var cant_Pendiente = new InstruccionesMetrics(is_creditor, id, 4);


            var t_Nofact = await _instruccionesDefRepository.CountAsync(cant_NoFacturado);
            var t_Facturado = await _instruccionesDefRepository.CountAsync(cant_Facturado);
            var t_FactAtraso = await _instruccionesDefRepository.CountAsync(cant_FacturadoAtraso);
            var t_Pendiente = await _instruccionesDefRepository.CountAsync(cant_Pendiente);

            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(cant_total);
            return new MetricsEstadoFacturadoDto
            {
                IDParticipante = id,
                TotalNoFacturado = t_Nofact,
                TotalFacturado = t_Facturado,
                TotalFacturadoConAtraso = t_FactAtraso,
                TotalPendiente = t_Pendiente,
                TotalMuestra = totalinstrucciones,
               
            };



        }


    }
}
