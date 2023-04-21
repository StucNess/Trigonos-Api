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
        private readonly IGenericRepository<REACT_CEN_banks> _banksRepository;
        private readonly IMapper _mapper;
        public BanksController(IGenericRepository<REACT_CEN_banks> banksRepository, IMapper mapper)
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
            //var maping = _mapper.Map<IReadOnlyList<REACT_CEN_banks>, IReadOnlyList<BanksDto>>(datos);
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
