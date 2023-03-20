using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIBanks")]
    public class BanksController : BaseApiController
    {
        private readonly IGenericRepository<CEN_banks> _banksRepository;
        private readonly IMapper _mapper;
        public BanksController(IGenericRepository<CEN_banks> banksRepository, IMapper mapper)
        {
            _banksRepository = banksRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IReadOnlyList<BanksDto>>> GetBanks()
        {
            var datos = await _banksRepository.GetAllAsync();
            //var maping = _mapper.Map<IReadOnlyList<CEN_banks>, IReadOnlyList<BanksDto>>(datos);
            return Ok(datos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BanksDto>> GetBank(int id)
        {

            var banco = await _banksRepository.GetByClienteIDAsync(id);
            //var producto = await _participantesRepository.GetByClienteIDAsync(spec);

            return Ok(banco);
        }

    }
}
