using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.DTO;

namespace TrigonosEnergy.Controllers
{

    public class ParticipantesController : BaseApiController
    {
        private readonly IGenericRepository<CEN_Participants> _participantesRepository;
        private readonly IMapper _mapper;
        public ParticipantesController(IGenericRepository<CEN_Participants> participantesRepository, IMapper mapper)
        {
            _participantesRepository = participantesRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<ParticipantesDTO>>> GetParticipantes()
        {
            var spec = new ParticipantsWithRelationSpecification();
            var producto = await _participantesRepository.GetAllAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<CEN_Participants>, IReadOnlyList<ParticipantesDTO>>(producto));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantesDTO>> GetParticipante(int id)
        {

            var spec = new ParticipantsWithRelationSpecification(id);
            var producto = await _participantesRepository.GetByClienteIDAsync(spec);

            return _mapper.Map<CEN_Participants, ParticipantesDTO>(producto);
        }
    }
}
