using AutoMapper;
using Core.Entities;
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
        private readonly IMapper _mapper;

        public InstruccionesController(IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository, IMapper mapper)
        {
            _instruccionesRepository = instruccionesRepository;
            _mapper = mapper;
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<InstruccionesDTO>>> GetInstrucciones(int id)
        //{
        //    var spec = new InstruccionesRelationSpecification(id);
        //    var producto = await _instruccionesRepository.GetAllInstrucctionByIdAsync(spec);

        //    return Ok(_mapper.Map<IReadOnlyList<TRGNS_Datos_Facturacion>, IReadOnlyList<InstruccionesDTO>>(producto));
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

        private decimal ConvertToDecimal(int v)
        {
            throw new NotImplementedException();
        }
    }
}
