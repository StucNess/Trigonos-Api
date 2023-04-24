using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;
using TrigonosEnergyWebAPI.Errors;

namespace TrigonosEnergyWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmpresasController : BaseApiController
    {

        private readonly IGenericRepository<REACT_TRGNS_Empresas> _empresasRepository;
        private readonly IMapper _mapper;
        public EmpresasController(IGenericRepository<REACT_TRGNS_Empresas> empresasRepository, IMapper mapper)
        {
            _empresasRepository = empresasRepository;
            _mapper = mapper;
        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<EmpresasDto>>> GetEmpresas()
        {
            var datos = await _empresasRepository.GetAllAsync();
            var maping = _mapper.Map<IReadOnlyList<REACT_TRGNS_Empresas>, IReadOnlyList<EmpresasDto>>(datos);
            return Ok(maping);
        }
        [HttpPost("Agregar")]
        public async Task<IActionResult> AgregarEmpresa(AgregarEmpresaDto agregarEmpresa)
        {
            var empresa = new REACT_TRGNS_Empresas
            {
                NombreEmpresa = agregarEmpresa.NombreEmpresa,
                RutEmpresa = agregarEmpresa.RutEmpresa,

            };

            if (!await _empresasRepository.SaveBD(empresa))
            {
                return BadRequest(new CodeErrorResponse(500, "Error no se ha agregado la empresa"));
            }
            return Ok();
        }
    }
}
