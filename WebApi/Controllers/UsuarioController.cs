using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioData _usuarioData;

        public UsuarioController(UsuarioData usuarioData)
        {
            _usuarioData = usuarioData;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos()
        {
            List<Usuario> listaUsuario = await _usuarioData.ListaUsuarios();
            return StatusCode(StatusCodes.Status200OK, listaUsuario);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            Usuario usuario = await _usuarioData.Usuario(id);
            return StatusCode(StatusCodes.Status200OK, usuario);
        }
    }
}
