using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    public class NominasController:BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TRGNS_Datos_Facturacion> _instruccionesRepository;
        //private readonly IGenericRepository<>
        public NominasController(IMapper mapper, IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository)
        {
            _mapper= mapper;
            _instruccionesRepository= instruccionesRepository;
        }
        [HttpGet]
        
        public async Task<ActionResult<Pagination<InstruccionesDTO>>> GetInstructionsOpen(int id, [FromQuery] NominasParamsSpecification parametros)
        {
            var spec = new NominasRelationSpecification(id, parametros);
            var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);
            var specCount = new NominasRelationSpecification(id, parametros);
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
    }
}
