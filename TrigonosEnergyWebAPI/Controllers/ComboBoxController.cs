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
    public class ComboBoxController : ControllerBase
    {
        private readonly IGenericRepository<CEN_dte_acceptance_status> _acceptanceRepository;
        private readonly IGenericRepository<CEN_payment_status_type> _paymentRepository;
        private readonly IGenericRepository<CEN_billing_types> _billingtypesRepository;
        
        private readonly IGenericRepository<CEN_billing_status_type> _billingRepository;
        private readonly IGenericRepository<TRGNS_dte_reception_status> _receptionRepository;
        private readonly IGenericRepository<TRGNS_Datos_Facturacion> _instruccionesRepository;
        private readonly IMapper _mapper;

        public ComboBoxController(IGenericRepository<CEN_dte_acceptance_status> acceptanceRepository, IMapper mapper,IGenericRepository<CEN_payment_status_type> paymentRepository,
            IGenericRepository<CEN_billing_status_type> billingRepository, IGenericRepository<TRGNS_dte_reception_status> receptionRepository, IGenericRepository<TRGNS_Datos_Facturacion> instruccionesRepository
            , IGenericRepository<CEN_billing_types> billingtypesRepository)
        {
            _acceptanceRepository = acceptanceRepository;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _billingRepository = billingRepository;
            _receptionRepository = receptionRepository;
            _instruccionesRepository = instruccionesRepository;
            _billingtypesRepository = billingtypesRepository;
        }

        //[HttpGet]
        //[Route("api/AcceptanceTypes")]
        //public async Task<ActionResult<List<ComboBox<CEN_dte_acceptance_status>>>> GetAceptacion()
        //{
        //    var producto = await _acceptanceRepository.GetAllAsync();
        //    return Ok(
        //        new ComboBox<CEN_dte_acceptance_status>
        //        {
        //            EstadoAceptacion = producto,

        //        }
        //        );
        //}
        //[HttpGet]
        //[Route("api/PaymentTypes")]
        //public async Task<ActionResult<List<ComboBox<CEN_payment_status_type>>>> GetPayment()
        //{
        //    var producto = await _paymentRepository.GetAllAsync();
        //    return Ok(
        //        new ComboBox<CEN_payment_status_type>
        //        {
        //            EstadoPago = producto,

        //        }
        //        );
        //}
        //[HttpGet]
        //[Route("api/BillingTypes")]
        //public async Task<ActionResult<List<ComboBox<CEN_billing_status_type>>>> GetBilling()
        //{
        //    var billing = await _billingRepository.GetAllAsync();

        //    return Ok(
        //        new ComboBox<CEN_billing_status_type>
        //        {
        //            EstadoFacturacion = billing,

        //        }
        //        );
        //}
        //public class ProductsCategory
        //{
        //    public IReadOnlyList<ComboBox<TRGNS_dte_reception_status>> Categories { get; set; }
        //    public IReadOnlyList<ComboBox<CEN_billing_status_type>> HOLA { get; set; }
        //}
        [HttpGet]
        public async Task<ActionResult<List<ComboBoxAPI>>> GetReception()
        {
            var reception = await _receptionRepository.GetAllAsync();
            var payment = await _paymentRepository.GetAllAsync();
            var acceptance = await _acceptanceRepository.GetAllAsync();
            var billing = await _billingRepository.GetAllAsync();
            var instruccion = await _instruccionesRepository.GetAllAsync();
            var billingtypes = await _billingtypesRepository.GetAllAsync();
            var instruccionDto = _mapper.Map<IReadOnlyList<CEN_billing_types>, IReadOnlyList<Concepto>>(billingtypes);
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
        //[Route("api/ReceptionTypes")]
        //public async Task<ActionResult<List<ComboBox<TRGNS_dte_reception_status>>>> GetReception()
        //{
        //    var producto = await _receptionRepository.GetAllAsync();
        //    return Ok(
        //        new ComboBox<TRGNS_dte_reception_status>
        //        {
        //            EstadoRecepcion = producto,

        //        }
        //        );
        //}
    }
}
