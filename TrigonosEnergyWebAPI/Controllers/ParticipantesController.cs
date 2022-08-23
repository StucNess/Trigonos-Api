using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.DTO;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergy.Controllers
{
    [ApiExplorerSettings(GroupName = "APIParticipantes")]
    public class ParticipantesController : BaseApiController
    {
        private readonly IGenericRepository<CEN_Participants> _participantesRepository;
        private readonly IGenericRepository<TRGNS_PROYECTOS> _proyectosRepository;
        private readonly IGenericRepository<TRGNS_H_CEN_participants> _pruebaRepo;
        private readonly IMapper _mapper;
        public ParticipantesController(IGenericRepository<CEN_Participants> participantesRepository, IMapper mapper, IGenericRepository<TRGNS_PROYECTOS> proyectosRepository,IGenericRepository<TRGNS_H_CEN_participants> pruebaRepo)
        {
            _participantesRepository = participantesRepository;
            _mapper = mapper;
            _proyectosRepository = proyectosRepository;
            _pruebaRepo = pruebaRepo;
        }
        /// <summary>
        /// Obtener a todos los participantes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ParticipantesDTO>>> GetParticipantes([FromQuery] ParticipantsParams parametros)
        {
            if (string.IsNullOrEmpty(parametros.All))
            {
                var spec = new ProyectosRelation();
                var participantes = await _proyectosRepository.GetAllAsync(spec);
                var specCount = new ProyectosRelation();
                var totalparticipantes = await _proyectosRepository.CountAsync(specCount);
                var rounded = Math.Ceiling(Convert.ToDecimal(totalparticipantes / parametros.PageSize));
                var totalPages = Convert.ToInt32(rounded);

                var data = _mapper.Map<IReadOnlyList<TRGNS_PROYECTOS>, IReadOnlyList<ParticipantesDTO>>(participantes);

                return Ok(
                    new Pagination<ParticipantesDTO>
                    {
                        count = totalparticipantes,
                        Data = data,
                        PageCount = totalPages,
                        PageIndex = parametros.PageIndex,
                        PageSize = parametros.PageSize,
                    }
                    );
            }
            else
            {
                var spec = new ParticipantsWithRelationSpecification(parametros);
                var participantes = await _participantesRepository.GetAllAsync(spec);
                var totalparticipantes = await _participantesRepository.CountAsync(spec);
                var rounded = Math.Ceiling(Convert.ToDecimal(totalparticipantes / parametros.PageSize));
                var totalPages = Convert.ToInt32(rounded);

                var data = _mapper.Map<IReadOnlyList<CEN_Participants>, IReadOnlyList<ParticipantesDTO>>(participantes);

                return Ok(
                    new Pagination<ParticipantesDTO>
                    {
                        count = totalparticipantes,
                        Data = data,
                        PageCount = totalPages,
                        PageIndex = parametros.PageIndex,
                        PageSize = parametros.PageSize,
                    }
                    );
            }
            
        }
        [HttpGet]
        [Route("/prueba")]
        public async Task<ActionResult<IReadOnlyList<TRGNS_H_CEN_participants>>> PRUEBA()
        {
            var datos = await _pruebaRepo.GetAllAsync();
            return Ok(datos);
        }
        /// <summary>
        /// Obtener un participante especifico
        /// </summary>
        /// <param name="id"> ID del participante</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantesDTO>> GetParticipante(int id)
        {

            var spec = new ParticipantsWithRelationSpecification(id);
            var producto = await _participantesRepository.GetByClienteIDAsync(spec);

            return _mapper.Map<CEN_Participants, ParticipantesDTO>(producto);
        }

        [HttpPatch]

        public async Task<IActionResult> Update(int id, [FromQuery] PatchParticipantsParams parametros)
        {
            var spec = new PatchParticipantsRelation(id);
            var prueba = await _participantesRepository.GetByClienteIDAsync(spec);
            var hist =new TRGNS_H_CEN_participants();
            hist.name_new = "hola";
            await _participantesRepository.SaveBD();
            
            if(parametros.Name != null) prueba.Name = parametros.Name;
            if (parametros.Rut != null) prueba.Rut = parametros.Rut;
            if (parametros.Verification_Code != null) prueba.Verification_Code = parametros.Verification_Code;
            if (parametros.Business_Name != null) prueba.Business_Name = parametros.Business_Name;
            if (parametros.Commercial_Business != null) prueba.Commercial_Business = parametros.Commercial_Business;
            if (parametros.Dte_Reception_Email != null) prueba.Dte_Reception_Email = parametros.Dte_Reception_Email;
            if (parametros.Bank_Account != null) prueba.Bank_Account = parametros.Bank_Account;
            if (parametros.bank != null) prueba.bank = parametros.bank;
            if (parametros.Commercial_address != null) prueba.Commercial_address = parametros.Commercial_address;
            if (parametros.Postal_address != null) prueba.Postal_address = parametros.Postal_address;
            if (parametros.Manager != null) prueba.Manager = parametros.Manager;
            if (parametros.Pay_Contact_First_Name != null) prueba.Pay_Contact_First_Name = parametros.Pay_Contact_First_Name;
            if (parametros.Pay_contact_last_name != null) prueba.Pay_contact_last_name = parametros.Pay_contact_last_name;
            if (parametros.Pay_contact_address != null) prueba.Pay_contact_address = parametros.Pay_contact_address;
            if (parametros.Pay_contact_phones != null) prueba.Pay_contact_phones = parametros.Pay_contact_phones;
            if (parametros.Pay_contact_email != null) prueba.Pay_contact_email = parametros.Pay_contact_email;
            if (parametros.Bills_contact_last_name != null) prueba.Bills_contact_last_name = parametros.Bills_contact_last_name;
            if (parametros.Bills_contact_first_name != null) prueba.Bills_contact_first_name = parametros.Bills_contact_first_name;
            if (parametros.Bills_contact_address != null) prueba.Bills_contact_address = parametros.Bills_contact_address;
            if (parametros.Bills_contact_phones != null) prueba.Bills_contact_phones = parametros.Bills_contact_phones;
            if (parametros.Bills_contact_email != null) prueba.Bills_contact_email = parametros.Bills_contact_email;





            if (!await _participantesRepository.UpdateeAsync(prueba))
            {
                return StatusCode(500);
            }
            return NoContent();



        }

    }
}
