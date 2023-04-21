using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Counting;
using Core.Specifications.Params;
using Core.Specifications.Relations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "APIDesconformidades")]
    public class NonconformitiesController : BaseApiController
    {
        private readonly IGenericRepository<REACT_CEN_nonconformities> _nonconformitiesRepository;
        private readonly IMapper _mapper;
        public NonconformitiesController(IGenericRepository<REACT_CEN_nonconformities> nonconformitiesRepository, IMapper mapper)
        {
            _nonconformitiesRepository = nonconformitiesRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Pagination<NonconformitiesDto>))]
        [ProducesResponseType(400)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IReadOnlyList<NonconformitiesDto>>> GetNonconformities([FromQuery] NonconformitiesSpecificationParams parametros)
        {
            //var datos = await _nonconformitiesRepository.GetAllAsync();
            //return Ok(datos);

            var spec = new NonconformitiesRelationSpecification(parametros);
            var nonconformities = await _nonconformitiesRepository.GetAllAsync(spec);
            var specCount = new NonconformitiesForCountingSpecification(parametros);
            var totalNonconformities = await _nonconformitiesRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalNonconformities / parametros.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<REACT_CEN_nonconformities>, IReadOnlyList<NonconformitiesDto>>(nonconformities);

            return Ok(
                new Pagination<NonconformitiesDto>
                {
                    count = totalNonconformities,
                    Data = data,
                    PageCount = totalPages + 1,
                    PageIndex = parametros.PageIndex,
                    PageSize = parametros.PageSize,
                }
                );
        }
    }
}
