using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TrigonosEnergy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantesController : BaseApiController
    {
        private readonly IGenericRepository<CEN_Participants> _participantesRepository;
        public ParticipantesController(IGenericRepository<CEN_Participants> participantesRepository)
        {
            _participantesRepository = participantesRepository;
        }
        [HttpGet]
        public async Task<ActionResult<List<CEN_Participants>>> GetParticipantes()
        {
            var spec = new ParticipantsWithRelationSpecification();
            var producto = await _participantesRepository.GetAllAsync(spec);
            return Ok(producto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CEN_Participants>> GetParticipante(int id)
        {
            //Spec = debe incluir la logica de la condicion del query y tambien debe incluir las relaciones entre las entidades
            //La relacion entre participante y bancos

            var spec = new ParticipantsWithRelationSpecification(id);
            return await _participantesRepository.GetByClienteIDAsync(spec);
        }
    }
}
