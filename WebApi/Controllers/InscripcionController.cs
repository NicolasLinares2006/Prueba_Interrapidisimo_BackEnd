using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly InscripcionData _inscripcionData;

        public InscripcionController(InscripcionData inscripcionData)
        {
            _inscripcionData = inscripcionData;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Inscripcion inscripcion)
        {
            var mensajes = await _inscripcionData.InscribirUsuario(inscripcion);

            if (mensajes.Contains("Usuario inscrito exitosamente."))
            {
                return StatusCode(StatusCodes.Status200OK, new
                {
                    isSuccess = true,
                    messages = mensajes
                });
            }

            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                isSuccess = false,
                messages = mensajes
            });
        }

    }
}
