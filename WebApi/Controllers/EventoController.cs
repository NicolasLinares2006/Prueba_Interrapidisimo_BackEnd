using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly EventoData _eventoData;

        public EventoController(EventoData eventoData)
        {
            _eventoData = eventoData;
        }

        [HttpGet]
        public async Task<IActionResult> GetEventos() {
            List<Evento> listaEvento = await _eventoData.ListaEventos();
            return StatusCode(StatusCodes.Status200OK, listaEvento);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvento(int id)
        {
            Evento evento = await _eventoData.Evento(id);
            return StatusCode(StatusCodes.Status200OK, evento);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Evento evento)
        {
            bool respuesta = await _eventoData.CrearEvento(evento);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Evento evento)
        {
            bool respuesta = await _eventoData.EditarEvento(evento);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool respuesta = await _eventoData.EliminarEvento(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
