using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Core.Specifications.Counting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.DTO;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;
using System.Text.RegularExpressions;

namespace TrigonosEnergy.Controllers
{
    [ApiExplorerSettings(GroupName = "APIParticipantes")]
    public class ParticipantesController : BaseApiController
    {
        private readonly IGenericRepository<REACT_CEN_Participants> _participantesRepository;
        private readonly IGenericRepository<REACT_TRGNS_PROYECTOS> _proyectosRepository;
        private readonly IGenericRepository<REACT_TRGNS_UserProyects> _proyectosUserRepository;
        private readonly IGenericRepository<REACT_TRGNS_H_CEN_participants> _pruebaRepo;
        private readonly IGenericRepository<REACT_TRGNS_FACTCLDATA> _factClRepository;
        private readonly IGenericRepository<REACT_TRGNS_AgentsOfParticipants> _agentsParticipantRepository;

        private readonly IMapper _mapper;
        public ParticipantesController(IGenericRepository<REACT_CEN_Participants> participantesRepository, IGenericRepository<REACT_TRGNS_AgentsOfParticipants> agentsParticipantRepository, IMapper mapper, IGenericRepository<REACT_TRGNS_PROYECTOS> proyectosRepository,IGenericRepository<REACT_TRGNS_H_CEN_participants> pruebaRepo, IGenericRepository<REACT_TRGNS_UserProyects> proyectosUserRepository, IGenericRepository<REACT_TRGNS_FACTCLDATA> factClRepository)
        {
            _participantesRepository = participantesRepository;
            _mapper = mapper;
            _proyectosRepository = proyectosRepository;
            _pruebaRepo = pruebaRepo;
            _proyectosUserRepository = proyectosUserRepository;
            _factClRepository = factClRepository;
            _agentsParticipantRepository = agentsParticipantRepository;
        }
       /// <summary>
       /// Obtener a los participantes de TRGNS o a todos los del CEN
       /// </summary>
       /// <param name="parametros"></param>
       /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Pagination<ParticipantesDTO>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<ParticipantesDTO>>> GetParticipantes(string? id,[FromQuery] ParticipantsParams parametros)
        {
            if (string.IsNullOrEmpty(parametros.All))
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var spec = new UserProyectsRelation(id);
                    var participantes = await _proyectosUserRepository.GetAllAsync(spec);
                    var specCount = new UserProyectsRelation(id);
                    var totalparticipantes = await _proyectosUserRepository.CountAsync(specCount);
                    var rounded = Math.Ceiling(Convert.ToDecimal(totalparticipantes / parametros.PageSize));
                    var totalPages = Convert.ToInt32(rounded);

                    var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_UserProyects>, IReadOnlyList<ParticipantesDTO>>(participantes);

                    return Ok(
                        new Pagination<ParticipantesDTO>
                        {
                            count = totalparticipantes,
                            Data = data,
                            PageCount = totalPages + 1,
                            PageIndex = parametros.PageIndex,
                            PageSize = parametros.PageSize,
                        }
                        );
                }
                else
                {
                    var spec = new ProyectosRelation();
                    var participantes = await _proyectosRepository.GetAllAsync(spec);
                    var specCount = new ProyectosRelation();
                    var totalparticipantes = await _proyectosRepository.CountAsync(specCount);
                    var rounded = Math.Ceiling(Convert.ToDecimal(totalparticipantes / parametros.PageSize));
                    var totalPages = Convert.ToInt32(rounded);

                    var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_PROYECTOS>, IReadOnlyList<ParticipantesDTO>>(participantes);

                    return Ok(
                        new Pagination<ParticipantesDTO>
                        {
                            count = totalparticipantes,
                            Data = data,
                            PageCount = totalPages + 1,
                            PageIndex = parametros.PageIndex,
                            PageSize = parametros.PageSize,
                        }
                        );
                }
                
            }
            else
            {
                var spec = new ParticipantsWithRelationSpecification(parametros);
                var specCount = new ParticipantsForCountingSpecification();
                var participantes = await _participantesRepository.GetAllAsync(spec);
                var totalparticipantes = await _participantesRepository.CountAsync(specCount);
                var rounded = Math.Ceiling(Convert.ToDecimal(totalparticipantes / parametros.PageSize));
                var totalPages = Convert.ToInt32(rounded);

                var data = _mapper.Map<IReadOnlyList<REACT_CEN_Participants>, IReadOnlyList<ParticipantesDTO>>(participantes);

                return Ok(
                    new Pagination<ParticipantesDTO>
                    {
                        count = totalparticipantes,
                        Data = data,
                        PageCount = totalPages+1,
                        PageIndex = parametros.PageIndex,
                        PageSize = parametros.PageSize,
                    }
                    );
            }
            
        }
        /// <summary>
        /// Cantidad de participantes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CantidadParticipantes")]
        public async Task<ActionResult<int>> CantidadParticipantes()
        {
            var specCount = new ParticipantsForCountingSpecification();
            var datos = await _participantesRepository.CountAsync(specCount);

            if (datos == 0)
            {
                return 0;
            }
            else
            {
                return Ok(datos);
            }


        }
        /// <summary>
        /// Cantidad de proyectos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CantidadProyectos")]
        public async Task<ActionResult<int>> CantidadProyectos()
        {
            var specCount = new ProyectosForCoutingSpec();
            var datos = await _proyectosRepository.CountAsync(specCount);

            if (datos == 0)
            {
                return 0;
            }
            else
            {
                return Ok(datos);
            }


        }
        /// <summary>
        /// Cantidad de participantes con facturacion cl
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CantidadFactCl")]
        public async Task<ActionResult<int>> CantidadFactCl()
        {
            var specCount = new FacturacionClForCoutingSpec();
            var datos = await _factClRepository.CountAsync(specCount);

            if (datos == 0)
            {
                return 0;
            }
            else
            {
                return Ok(datos);
            }


        }
        /// <summary>
        /// Retorna si es de bluetree o es externo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ProyectosBy/{id}")]
        public async Task<ActionResult<REACT_TRGNS_PROYECTOS>> ProyectosById(int id)
        {
            var datos = await _proyectosRepository.GetAllAsync();


           
            var busqueda = datos.FirstOrDefault(i => i.Id_participants == id);
            if (busqueda == null)
            {
                return NotFound(new CodeErrorResponse(404, "La pagina web no existe"));
            }
            else
            {
                return busqueda;
            }

        
        }
        /// <summary>
        /// Retorna los agentes asociados a un participante
        /// </summary>
        /// <returns></returns>
        [HttpGet("AgentesDeParticipante")]
        public async Task<ActionResult<Pagination<AgentsParticipantsDto>>> GetAgentesOfParticipants([FromQuery] AgentesSpecificationParams agentesParams)
        {

            var spec = new AgentesSpecification(agentesParams);
            var specTotal = new AgentesSpecification(agentesParams.rutEmpresa);

            var datos = await _agentsParticipantRepository.GetAllAsync(spec);
            var datosTotal = await _agentsParticipantRepository.GetAllAsync(specTotal);
            var rounded = Math.Ceiling(Convert.ToDecimal(datosTotal.Count() / agentesParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_AgentsOfParticipants>, IReadOnlyList<AgentsParticipantsDto>>(datos);
            return Ok(
                new Pagination<AgentsParticipantsDto>
                {
                    count = datosTotal.Count(),
                    Data = maping,
                    PageCount = totalPages + 1,
                    PageIndex = agentesParams.PageIndex,
                    PageSize = agentesParams.PageSize,
                }
                );
        }
        /// <summary>
        /// Retorna todos los si es de bluetree o es externo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Proyectos")]
        public async Task<IReadOnlyList<REACT_TRGNS_PROYECTOS>> GetAllProyectos()
        {
            var datos = await _proyectosRepository.GetAllAsync();



            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_PROYECTOS>, IReadOnlyList<REACT_TRGNS_PROYECTOS>>(datos);
            return maping;



        }
        [HttpGet("PaginationProyectos")]
        public async Task<ActionResult<Pagination<TrgnsProyectosDto>>> GetPaginationEncodeFacturacionCl([FromQuery] ProyectosSpecificationParams proyecParams)
        {

            var spec = new ProyectosSpecification(proyecParams);

            var datos = await _proyectosRepository.GetAllAsync(spec);
            var total = datos.Count();
            var rounded = Math.Ceiling(Convert.ToDecimal(total / proyecParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_PROYECTOS>, IReadOnlyList<TrgnsProyectosDto>>(datos);
            return Ok(
                new Pagination<TrgnsProyectosDto>
                {
                    count = total,
                    Data = data,
                    PageCount = totalPages + 1,
                    PageIndex = proyecParams.PageIndex,
                    PageSize = proyecParams.PageSize,
                }
                );
        }

        /// <summary>
        /// actualización dinamica de los participantes en la tabla proyectos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ActualizarProyecto")]
        public async Task<ActionResult<bool>> ActivarProyectoById(ActualizarProyectos actproyect)
        {
            var datos = await _proyectosRepository.GetAllAsync();



            var busqueda = datos.FirstOrDefault(i => i.Id_participants == actproyect.Id_participants);


            if (busqueda == null)
            {

                //agregar a la bd si el id que trae es verdadero y esta en participantes


                var participante = await _participantesRepository.GetByClienteIDAsync(actproyect.Id_participants);

                if(participante != null)
                {

                    var newproyect = new REACT_TRGNS_PROYECTOS
                    {
                        Id_participants = actproyect.Id_participants,
                        Erp = actproyect.Erp != null ? actproyect.Erp : 9,
                        Group = 0,
                        vHabilitado = actproyect.vHabilitado != null ? actproyect.vHabilitado : 0,
                        Id_nomina_pago = actproyect.Id_nomina_pago != null ? actproyect.Id_nomina_pago : 4,



                    };
                  

                    if (!await _proyectosRepository.SaveBD(newproyect))
                    {
                        return BadRequest(new CodeErrorResponse(500, "Error al agregar el proyecto"));
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return NotFound(new CodeErrorResponse(404, "el proyecto y el participante no existe "));
                }
                

                
            }
            else
            {
                var lista = await _factClRepository.GetAllAsync();
                var eliminar = lista.FirstOrDefault(i => i.IdParticipante == actproyect.Id_participants);
                if(eliminar != null)
                {
                    if (actproyect.Erp != 5 && actproyect.Erp != null )
                    {
                        try
                        {

                            var resp = await _factClRepository.RemoveBD(eliminar);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                }
                

                busqueda.Erp = actproyect.Erp != null ? actproyect.Erp : busqueda.Erp;
                busqueda.vHabilitado = actproyect.vHabilitado != null ? (busqueda.vHabilitado == 0 ? 1 : 0 ): busqueda.vHabilitado ;
                busqueda.Id_nomina_pago = actproyect.Id_nomina_pago != null ? actproyect.Id_nomina_pago : busqueda.Id_nomina_pago;

                if (!await _proyectosRepository.UpdateeAsync(busqueda))
                {
                    return StatusCode(500);
                }
                else
                {
                    return true;
                }

            }


        }
        /// <summary>
        /// actualiza para agregar como bluetree
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ActHabilitadoProyect/{id}")]
        public async Task<ActionResult<bool>> ActualizarProyectoById(int id)
        {
            var datos = await _proyectosRepository.GetAllAsync();



            var busqueda = datos.FirstOrDefault(i => i.Id_participants == id);

            if (busqueda == null)
            {
                return NotFound(new CodeErrorResponse(404, "el proyecto no existe"));
            }
            else
            {
                busqueda.vHabilitado = 1;

                if (!await _proyectosRepository.UpdateeAsync(busqueda))
                {
                    return StatusCode(500);
                }
                else
                {
                    return true;
                }

            }


        }
        /// <summary>
        /// actualiza para eliminar de clientes bluetree (desactivar)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DesacHabilitadoProyect/{id}")]
        public async Task<ActionResult<bool>> DesactivarProyectoById(int id)
        {
            var datos = await _proyectosRepository.GetAllAsync();



            var busqueda = datos.FirstOrDefault(i => i.Id_participants == id);

            if (busqueda == null)
            {
                return NotFound(new CodeErrorResponse(404, "el proyecto no existe"));
            }
            else
            {
                busqueda.vHabilitado = 0;

                if (!await _proyectosRepository.UpdateeAsync(busqueda))
                {
                    return StatusCode(500);
                }
                else
                {
                    return true;
                }

            }


        }
        /// <summary>
        /// Obtener el nombre comercial de todos los participantes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/BusinessName")]
        public async Task<ActionResult<IReadOnlyList<REACT_CEN_Participants>>> PRUEBA()
        {
            var datos = await _participantesRepository.GetAllAsync();
            var maping = _mapper.Map <IReadOnlyList<REACT_CEN_Participants>, IReadOnlyList<BusinessNameDto>>(datos);
            return Ok(maping);
        }
        /// <summary>
        /// Obtener el rut de todos los participantes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Rut")]
        public async Task<ActionResult<IReadOnlyList<REACT_CEN_Participants>>> PRUEBA1()
        {
            var datos = await _participantesRepository.GetAllAsync();
            var maping = _mapper.Map<IReadOnlyList<REACT_CEN_Participants>, IReadOnlyList<RutDto>>(datos);
            return Ok(maping);
        }
        /// <summary>
        /// Obtener un participante especifico
        /// </summary>
        /// <param name="id"> ID del participante</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ParticipantesDTO>> GetParticipante(int id,[FromQuery] ParticipantsParamsID parametros)
        {

            var spec = new ParticipantsWithRelationSpecification(id, parametros);
            var producto = await _participantesRepository.GetByClienteIDAsync(spec);

            return _mapper.Map<REACT_CEN_Participants, ParticipantesDTO>(producto);
        }

        [HttpGet]
        [Route("/Historificacion/{id}")]
        public async Task<ActionResult<Pagination<HistorificacionDto>>> GetParticipanteHistorico(int id, [FromQuery] HistorificacionParams parametros)
        {

            var spec = new HistorificacionParticipantesSpecification(id, parametros);
            var datos = await _pruebaRepo.GetAllAsync(spec);
           
            var specCount = new HistorificacionParticipantesSpecification(id);
            var totalHist = await _pruebaRepo.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalHist / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<REACT_TRGNS_H_CEN_participants>, IReadOnlyList<HistorificacionDto>>(datos);


            return Ok(
                new Pagination<HistorificacionDto>
                {
                    count = totalHist,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,



                }
                );
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
            var bdh = new REACT_TRGNS_H_CEN_participants();
            bdh.name_old = "0";
            bdh.name_new = "0";
            bdh.rut_old = "0";
            bdh.rut_new = "0";
            bdh.verification_code_old = "0";
            bdh.verification_code_new = "0";
            bdh.business_name_old = "0";
            bdh.business_name_new = "0";
            bdh.commercial_business_old = "0";
            bdh.commercial_business_new = "0";
            bdh.dte_reception_email_new = "0";
            bdh.dte_reception_email_old = "0";
            bdh.bank_account_old = "0";
            bdh.bank_account_new = "0";
            bdh.bank_old = 0;
            bdh.bank_new = 0;
            bdh.commercial_address_old = "0";
            bdh.commercial_address_new = "0";
            bdh.postal_address_old = "0";
            bdh.postal_address_new = "0";
            bdh.manager_old = "0";
            bdh.manager_new = "0";
            bdh.pay_contact_first_name_old = "0";
            bdh.pay_contact_first_name_new = "0";
            bdh.pay_contact_last_name_old = "0";
            bdh.pay_contact_last_name_new = "0";
            bdh.pay_contact_address_old = "0";
            bdh.pay_contact_address_new = "0";
            bdh.pay_contact_phones_old = "0";
            bdh.pay_contact_phones_new = "0";
            bdh.pay_contact_email_old = "0";
            bdh.pay_contact_email_new = "0";
            bdh.bills_contact_first_name_old = "0";
            bdh.bills_contact_first_name_new = "0";
            bdh.bills_contact_last_name_old = "0";
            bdh.bills_contact_last_name_new = "0";
            bdh.bills_contact_address_old = "0";
            bdh.bills_contact_address_new = "0";
            bdh.bills_contact_phones_old = "0";
            bdh.bills_contact_phones_new = "0";
            bdh.bills_contact_email_old = "0";
            bdh.bills_contact_email_new = "0";
            bdh.created_ts_old = "0";
            bdh.created_ts_new = "0";

            //

            try
            {


                bdh.id_definir = id;
                bdh.editor = "USUARIO TRGNS";
                bdh.date = DateTime.Now;

                if (parametros.Name != bd.Name && parametros.Name != null)
                {
                    bdh.name_old = bd.Name;
                    bd.Name =parametros.Name;
                    bdh.name_new = parametros.Name;


                }
  
                if (parametros.Rut != bd.Rut && parametros.Rut != null)
                {
                    bdh.rut_old = bd.Rut;
                    bd.Rut = parametros.Rut;
                    bdh.rut_new = parametros.Rut;
                }
                if (parametros.Verification_Code != bd.Verification_Code && parametros.Verification_Code != null)
                {
                    bdh.verification_code_old = bd.Verification_Code;
                    bd.Verification_Code = parametros.Verification_Code;
                    bdh.verification_code_new = parametros.Verification_Code;
                }
                if (parametros.Business_Name != bd.Business_Name && parametros.Business_Name != null)
                {
                    bdh.business_name_old = bd.Business_Name;
                    bd.Business_Name = parametros.Business_Name;
                    bdh.business_name_new = parametros.Business_Name;
                }

                if (parametros.Commercial_Business != bd.Commercial_Business && parametros.Commercial_Business != null)
                {
                    bdh.commercial_business_old = bd.Commercial_Business;
                    bd.Commercial_Business = parametros.Commercial_Business;
                    bdh.commercial_business_new = parametros.Commercial_Business;
                }
                if (parametros.Dte_Reception_Email != bd.Dte_Reception_Email && parametros.Dte_Reception_Email != null)
                {
                    bdh.dte_reception_email_old = bd.Dte_Reception_Email;
                    bd.Dte_Reception_Email = parametros.Dte_Reception_Email;
                    bdh.dte_reception_email_new = parametros.Dte_Reception_Email;
                }
                if (parametros.Bank_Account != bd.Bank_Account && parametros.Bank_Account != null)
                {
                    bdh.bank_account_old = bd.Bank_Account;
                    bd.Bank_Account = parametros.Bank_Account;
                    bdh.bank_account_new = parametros.Bank_Account;
                }
                if (parametros.bank != bd.bank && parametros.bank != null)
                {
                    bdh.bank_old = bd.bank;
                    bd.bank = parametros.bank;
                    bdh.bank_new = parametros.bank;
                }
                if (parametros.Commercial_address != bd.Commercial_address && parametros.Commercial_address != null)
                {
                    bdh.commercial_address_old = bd.Commercial_address;
                    bd.Commercial_address = parametros.Commercial_address;
                    bdh.commercial_address_new = parametros.Commercial_address;
                }
                if (parametros.Postal_address != bd.Postal_address && parametros.Postal_address != null)
                {
                    bdh.postal_address_old = bd.Postal_address;
                    bd.Postal_address = parametros.Postal_address;
                    bdh.postal_address_new = parametros.Postal_address;
                }
                if (parametros.Manager != bd.Manager && parametros.Manager != null)
                {
                    bdh.manager_old = bd.Manager;
                    bd.Manager = parametros.Manager;
                    bdh.manager_new = parametros.Manager;
                }
                if (parametros.Pay_Contact_First_Name != bd.Pay_Contact_First_Name && parametros.Pay_Contact_First_Name != null)
                {
                    bdh.pay_contact_first_name_old = bd.Pay_Contact_First_Name;
                    bd.Pay_Contact_First_Name = parametros.Pay_Contact_First_Name;
                    bdh.pay_contact_first_name_new = parametros.Pay_Contact_First_Name;
                }
                if (parametros.Pay_contact_last_name != bd.Pay_contact_last_name && parametros.Pay_contact_last_name != null)
                {
                    bdh.pay_contact_last_name_old = bd.Pay_contact_last_name;
                    bd.Pay_contact_last_name = parametros.Pay_contact_last_name;
                    bdh.pay_contact_last_name_new = parametros.Pay_contact_last_name;
                }
                if (parametros.Pay_contact_address != bd.Pay_contact_address && parametros.Pay_contact_address != null)
                {
                    bdh.pay_contact_address_old = bd.Pay_contact_address;
                    bd.Pay_contact_address = parametros.Pay_contact_address;
                    bdh.pay_contact_address_new = parametros.Pay_contact_address;
                }
                if (parametros.Pay_contact_phones != bd.Pay_contact_phones && parametros.Pay_contact_phones != null)
                {
                    bdh.pay_contact_phones_old = bd.Pay_contact_phones;
                    bd.Pay_contact_phones = parametros.Pay_contact_phones;
                    bdh.pay_contact_phones_new = parametros.Pay_contact_phones;
                }
                if (parametros.Pay_contact_email != bd.Pay_contact_email && parametros.Pay_contact_email != null)
                {
                    bdh.pay_contact_email_old = bd.Pay_contact_email;
                    bd.Pay_contact_email = parametros.Pay_contact_email;
                    bdh.pay_contact_email_new = parametros.Pay_contact_email;
                }
                if (parametros.Bills_contact_last_name != bd.Bills_contact_last_name && parametros.Bills_contact_last_name != null)
                {
                    bdh.bills_contact_last_name_old = bd.Bills_contact_last_name;
                    bd.Bills_contact_last_name = parametros.Bills_contact_last_name;
                    bdh.bills_contact_last_name_new = parametros.Bills_contact_last_name;
                }
                if (parametros.Bills_contact_first_name != bd.Bills_contact_first_name && parametros.Bills_contact_first_name != null)
                {
                    bdh.bills_contact_first_name_old = bd.Bills_contact_first_name;
                    bd.Bills_contact_first_name = parametros.Bills_contact_first_name;
                    bdh.bills_contact_first_name_new = parametros.Bills_contact_first_name;
                }
                if (parametros.Bills_contact_address != bd.Bills_contact_address && parametros.Bills_contact_address != null)
                {
                    bdh.bills_contact_address_old = bd.Bills_contact_address;
                    bd.Bills_contact_address = parametros.Bills_contact_address;
                    bdh.bills_contact_address_new = parametros.Bills_contact_address;
                }

                if (parametros.Bills_contact_phones != bd.Bills_contact_phones && parametros.Bills_contact_phones != null)
                {
                    bdh.bills_contact_phones_old = bd.Bills_contact_phones;
                    bd.Bills_contact_phones = parametros.Bills_contact_phones;
                    bdh.bills_contact_phones_new = parametros.Bills_contact_phones;
                }
                if (parametros.Bills_contact_email != bd.Bills_contact_email && parametros.Bills_contact_email != null)
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
