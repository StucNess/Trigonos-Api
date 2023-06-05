using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APINominas")]
    public class NominasController:BaseApiController
    {
        private readonly IMapper _mapper;
        //private readonly IGenericRepository<REACT_TRGNS_Datos_Facturacion> _instruccionesRepository;
        private readonly IGenericRepository<REACT_CEN_nonconformities> _nonconformitiesRepository;
        private readonly IGenericRepository<REACT_CEN_instructions_Def> _instruccionesDefRepository;
        private readonly IGenericRepository<REACT_TRGNS_NominaPagos> _nominapagosRepository;
        private readonly IGenericRepository<REACT_TRGNS_Erp> _facturadorerpRepository;
        //private readonly IGenericRepository<>
        public NominasController(IMapper mapper, IGenericRepository<REACT_TRGNS_NominaPagos> nominapagosRepository, IGenericRepository<REACT_TRGNS_Erp> facturadorerpRepository, IGenericRepository<REACT_CEN_instructions_Def> instruccionesDefRepository, IGenericRepository<REACT_CEN_nonconformities> nonconformitiesRepository)
        {
            _mapper= mapper;
            _instruccionesDefRepository= instruccionesDefRepository;
            _nonconformitiesRepository= nonconformitiesRepository;
            _nominapagosRepository = nominapagosRepository;
            _facturadorerpRepository = facturadorerpRepository;
        }
        [HttpGet]

        public async Task<ActionResult<Pagination<NominasBciDto>>> GetInstructionsOpen(int id, [FromQuery] NominasParamsSpecification parametros)
        {
            var spec = new NominasRelationSpecification(id, parametros);
            var producto = await _instruccionesDefRepository.GetAllInstrucctionByIdAsync(spec);
            var specCount = new NominasForCountingSpecification(id, parametros);
            var totalinstrucciones = await _instruccionesDefRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalinstrucciones / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<REACT_CEN_instructions_Def>, IReadOnlyList<NominasBciDto>>(producto);
            //return Ok(
            //   data
            //    );

            return Ok(
                new Pagination<NominasBciDto>
                {
                    count = totalinstrucciones,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,



                    //    }
                    //    );
                });
        }

        /// <summary>
        /// Obtener nominas que trabaja Prisma
        /// </summary>
        [HttpGet("NominasPago")]

        public async Task<IReadOnlyList<NominaPagosDto>> GetAllNominaPago()
        {

            var nominas = await _nominapagosRepository.GetAllAsync();

            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_NominaPagos>, IReadOnlyList<NominaPagosDto>>(nominas);
            return maping;
        }

        /// <summary>
        /// Obtener nominas que trabaja Prisma
        /// </summary>
        [HttpGet("FacturadorERP")]

        public async Task<IReadOnlyList<FacturadorErpDto>> GetAllFacturadorERP()
        {

            var nominas = await _facturadorerpRepository.GetAllAsync();

            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_Erp>, IReadOnlyList<FacturadorErpDto>>(nominas);
            return maping;
        }
    }
}
