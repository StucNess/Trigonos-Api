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

        [HttpGet("{id}")]
        public async Task<ActionResult<InstruccionesDTO>> GetInstrucciones(int id)
        {
            var spec = new InstruccionesRelationSpecification(id);
            var producto = await _instruccionesRepository.GetByClienteIDAsync(spec);

            return _mapper.Map<TRGNS_Datos_Facturacion, InstruccionesDTO>(producto);
        }

    }
}
