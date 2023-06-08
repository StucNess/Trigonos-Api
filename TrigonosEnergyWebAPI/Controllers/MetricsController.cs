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
        public async Task<ActionResult<MetricInstruccionDto>> MetricaPrueba(int id, bool? is_creditor)
        {

            var cant_total = new InstruccionesMetrics(id, null, is_creditor);
            var cant_no_pagado = new InstruccionesMetrics(id, 1, is_creditor);
            var cant_pagado = new InstruccionesMetrics(id, 2, is_creditor);
            var cant_pago_atrasado = new InstruccionesMetrics(id, 3, is_creditor);

            var t_pagado = await _instruccionesDefRepository.CountAsync(cant_pagado);
            var t_no_pagado = await _instruccionesDefRepository.CountAsync(cant_no_pagado);
            var t_atrasado = await _instruccionesDefRepository.CountAsync(cant_pago_atrasado);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(cant_total);
            return new MetricInstruccionDto
            {
                IDParticipante =  id,
                TotalNoPagado = t_no_pagado,
                TotalPagado = t_pagado,
                TotalAtrasado = t_atrasado,
                TotalMuestra = totalinstrucciones,
            };



        }


    }
}
