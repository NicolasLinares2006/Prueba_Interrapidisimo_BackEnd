using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Evento
    {
        public int Id { get; set; } 

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; } 

        public DateTime FechaHora { get; set; } 

        public string? Ubicacion { get; set; }

        public int CapacidadMaxima { get; set; } 

        public int UsuarioId { get; set; }
        public int Asistentes { get; set; }

        public Usuario Usuario { get; set; }
    }
}
