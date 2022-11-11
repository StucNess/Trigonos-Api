using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "APIComboBox")]
    public class ComboBoxController : ControllerBase
    {
        private readonly IGenericRepository<CEN_dte_acceptance_status> _acceptanceRepository;
        private readonly IGenericRepository<CEN_payment_status_type> _paymentRepository;
        private readonly IGenericRepository<CEN_billing_windows> _billingwindowsRepository;

        private readonly IGenericRepository<CEN_billing_status_type> _billingRepository;
        private readonly IGenericRepository<TRGNS_dte_reception_status> _receptionRepository;
        private readonly IGenericRepository<TRGNS_Datos_Facturacion> _instruccionesRepository;
        private readonly IMapper _mapper;

        public ComboBoxController(IGenericRepository<CEN_dte_acceptance_status> acceptanceRepository, IMapper mapper, IGenericRepository<CEN_payment_status_type> paymentRepository,
            IGenericRepository<CEN_billing_status_type> billingRepository, IGenericRepository<TRGNS_dte_reception_status> receptionRepository, IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository
            , IGenericRepository<CEN_billing_windows> billingwindowsRepository)
        {
            _acceptanceRepository = acceptanceRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _billingRepository = billingRepository;
            _receptionRepository = receptionRepository;
            _instruccionesRepository = instruccionesRepository;
            _billingwindowsRepository = billingwindowsRepository;
        }


        /// <summary>
        /// Api para llenar datos de los ComboBox
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<ComboBoxAPI>))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<ComboBoxAPI>>> GetReception()
        {
            var reception = await _receptionRepository.GetAllAsync();
            var payment = await _paymentRepository.GetAllAsync();
            var acceptance = await _acceptanceRepository.GetAllAsync();
            var billing = await _billingRepository.GetAllAsync();
            var instruccion = await _instruccionesRepository.GetAllAsync();
            var billingtypes = await _billingwindowsRepository.GetAllAsync();
            var instruccionDto = _mapper.Map<IReadOnlyList<CEN_billing_windows>, IReadOnlyList<Concepto>>(billingtypes);
            return Ok(
                new ComboBoxAPI
                {
                    EstadoFacturacion = billing,
                    EstadoRecepcion = reception,
                    EstadoPago = payment,
                    EstadoAceptacion = acceptance,
                    Concepto = instruccionDto,



                }
                );
        }
        //[HttpGet]
        //[Route("/windows")]
        //public async Task<ActionResult<IReadOnlyList<CEN_billing_windows>>> PRUEBA()
        //{

        //    var billingtypes = await _billingwindowsRepository.GetAllAsync();
        //    var instruccionDto = _mapper.Map<IReadOnlyList<CEN_billing_windows>, IReadOnlyList<BillingWindowsDto>>(billingtypes);
        //    return Ok(instruccionDto);
        //}
    } 
}
