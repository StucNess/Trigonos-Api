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
       /// Obtener a los participantes de TRGNS o a todos los del CEN
       /// </summary>
       /// <param name="parametros"></param>
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
        //[HttpGet]
        //[Route("/prueba")]
        //public async Task<ActionResult<IReadOnlyList<TRGNS_H_CEN_participants>>> PRUEBA()
        //{
        //    var datos = await _pruebaRepo.GetAllAsync();
        //    return Ok(datos);
        //}
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
        /// <summary>
        /// Actualizar datos de un participante
        /// </summary>
        /// <param name="id">ID del participante</param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPatch]

        public async Task<IActionResult> Update(int id, [FromQuery] PatchParticipantsParams parametros)
        {
            var spec = new PatchParticipantsRelation(id);
            var bdc = await _participantesRepository.GetByClienteIDAsync(spec);
            var bd= await _participantesRepository.GetByClienteIDAsync(spec);
            var bdh = new TRGNS_H_CEN_participants();
            //

            try
            {


                bdh.id_definir = id;
                bdh.editor = "USUARIO TRGNS";
                bdh.date = DateTime.Now;
                if (parametros.Name != null)
                {

                    bdh.name_old = bd.Name;
                    bd.Name = parametros.Name;
                    bdh.name_new = parametros.Name;


                };
                if (parametros.Rut != null)
                {
                    bdh.rut_old = bd.Rut;
                    bd.Rut = parametros.Rut;
                    bdh.rut_new = parametros.Rut;
                }
                if (parametros.Verification_Code != null)
                {
                    bdh.verification_code_old = bd.Verification_Code;
                    bd.Verification_Code = parametros.Verification_Code;
                    bdh.verification_code_new = parametros.Verification_Code;
                }
                if (parametros.Business_Name != null)
                {
                    bdh.business_name_old = bd.Business_Name;
                    bd.Business_Name = parametros.Business_Name;
                    bdh.business_name_new = parametros.Business_Name;
                }
                if (parametros.Commercial_Business != null)
                {
                    bdh.commercial_business_old = bd.Commercial_Business;
                    bd.Commercial_Business = parametros.Commercial_Business;
                    bdh.commercial_business_new = parametros.Commercial_Business;
                }
                if (parametros.Dte_Reception_Email != null)
                {
                    bdh.dte_reception_email_old = bd.Dte_Reception_Email;
                    bd.Dte_Reception_Email = parametros.Dte_Reception_Email;
                    bdh.dte_reception_email_new = parametros.Dte_Reception_Email;
                }
                if (parametros.Bank_Account != null)
                {
                    bdh.bank_account_old = bd.Bank_Account;
                    bd.Bank_Account = parametros.Bank_Account;
                    bdh.bank_account_new = parametros.Bank_Account;
                }
                if (parametros.bank != null)
                {
                    bdh.bank_old = bd.bank;
                    bd.bank = parametros.bank;
                    bdh.bank_new = parametros.bank;
                }
                if (parametros.Commercial_address != null)
                {
                    bdh.commercial_address_old = bd.Commercial_address;
                    bd.Commercial_address = parametros.Commercial_address;
                    bdh.commercial_address_new = parametros.Commercial_address;
                }
                if (parametros.Postal_address != null)
                {
                    bdh.postal_address_old = bd.Postal_address;
                    bd.Postal_address = parametros.Postal_address;
                    bdh.postal_address_new = parametros.Postal_address;
                }
                if (parametros.Manager != null)
                {
                    bdh.manager_old = bd.Manager;
                    bd.Manager = parametros.Manager;
                    bdh.manager_new = parametros.Manager;
                }
                if (parametros.Pay_Contact_First_Name != null)
                {
                    bdh.pay_contact_first_name_old = bd.Pay_Contact_First_Name;
                    bd.Pay_Contact_First_Name = parametros.Pay_Contact_First_Name;
                    bdh.pay_contact_first_name_new = parametros.Pay_Contact_First_Name;
                }
                if (parametros.Pay_contact_last_name != null)
                {
                    bdh.pay_contact_last_name_old = bd.Pay_contact_last_name;
                    bd.Pay_contact_last_name = parametros.Pay_contact_last_name;
                    bdh.pay_contact_last_name_new = parametros.Pay_contact_last_name;
                }
                if (parametros.Pay_contact_address != null)
                {
                    bdh.pay_contact_address_old = bd.Pay_contact_address;
                    bd.Pay_contact_address = parametros.Pay_contact_address;
                    bdh.pay_contact_address_new = parametros.Pay_contact_address;
                }
                if (parametros.Pay_contact_phones != null)
                {
                    bdh.pay_contact_phones_old = bd.Pay_contact_phones;
                    bd.Pay_contact_phones = parametros.Pay_contact_phones;
                    bdh.pay_contact_phones_new = parametros.Pay_contact_phones;
                }
                if (parametros.Pay_contact_email != null)
                {
                    bdh.pay_contact_email_old = bd.Pay_contact_email;
                    bd.Pay_contact_email = parametros.Pay_contact_email;
                    bdh.pay_contact_email_new = parametros.Pay_contact_email;
                }
                if (parametros.Bills_contact_last_name != null)
                {
                    bdh.bills_contact_last_name_old = bd.Bills_contact_last_name;
                    bd.Bills_contact_last_name = parametros.Bills_contact_last_name;
                    bdh.bills_contact_last_name_new = parametros.Bills_contact_last_name;
                }
                if (parametros.Bills_contact_first_name != null)
                {
                    bdh.bills_contact_first_name_old = bd.Bills_contact_first_name;
                    bd.Bills_contact_first_name = parametros.Bills_contact_first_name;
                    bdh.bills_contact_first_name_new = parametros.Bills_contact_first_name;
                }
                if (parametros.Bills_contact_address != null)
                {
                    bdh.bills_contact_address_old = bd.Bills_contact_address;
                    bd.Bills_contact_address = parametros.Bills_contact_address;
                    bdh.bills_contact_address_new = parametros.Bills_contact_address;
                }

                if (parametros.Bills_contact_phones != null)
                {
                    bdh.bills_contact_phones_old = bd.Bills_contact_phones;
                    bd.Bills_contact_phones = parametros.Bills_contact_phones;
                    bdh.bills_contact_phones_new = parametros.Bills_contact_phones;
                }
                if (parametros.Bills_contact_email != null)
                {
                    bdh.bills_contact_email_old = bd.Bills_contact_email;
                    bd.Bills_contact_email = parametros.Bills_contact_email;
                    bdh.bills_contact_email_new = parametros.Bills_contact_email;
                }
                bdh.updated_ts_old = bd.Updated_ts;
                bd.Updated_ts = DateTime.Now.ToString();
                bdh.updated_ts_new = DateTime.Now.ToString();
            }
            catch (Exception)
            {
                throw;

            }


            var guardar = await _pruebaRepo.SaveBD(bdh);







            if (!await _participantesRepository.UpdateeAsync(bd))
            {
                return StatusCode(500);
            }
            return NoContent();





        }

    }
}
