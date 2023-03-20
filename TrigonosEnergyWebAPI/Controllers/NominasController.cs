using AutoMapper;
using Core.Entities;
using Core.Interface;
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
        
        public async Task<ActionResult<Pagination<InstruccionesDTO>>> GetInstructionsOpen(int id, [FromQuery] Instru)
        {

        }
    }
}
