using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrigonosEnergy.Controllers;
using TrigonosEnergyWebAPI.DTO;

namespace TrigonosEnergyWebAPI.Controllers
{

    public class UsuarioController : BaseApiController
    {
        private readonly IRepositoryUsuario _repo;
        private readonly IMapper _mapper;

        public UsuarioController(IRepositoryUsuario repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var listaUsuarios = _repo.GetUsuarios();
            var listaUsuariosDto = new List<UsuarioDto>();
            foreach (var lista in listaUsuarios)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(lista));
            }
            return Ok(listaUsuariosDto);
        }
        [HttpGet("{Id:int}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int Id)
        {
            var itemUsuario = _repo.GetUsuario(Id);
            if (itemUsuario == null)
            {
                return NotFound();
            }
            var itemUsuarioDto = _mapper.Map<UsuarioDto>(itemUsuario);
            return Ok(itemUsuarioDto);
        }
        [HttpPost("Registro")]
        public IActionResult Registro(UsuarioAuthDto usuarioAuthDto)
        {
            usuarioAuthDto.Usuario = usuarioAuthDto.Usuario.ToLower();
            if (_repo.ExisteUsuario(usuarioAuthDto.Usuario))
            {
                return BadRequest("El usuario ya existe");
            }
            var usuarioACrear = new Usuario
            {
                UsuarioA = usuarioAuthDto.Usuario
            };
            var usuarioCreado = _repo.Registro(usuarioACrear,usuarioAuthDto.Password);
            return Ok(usuarioCreado);
        }

    }
}
