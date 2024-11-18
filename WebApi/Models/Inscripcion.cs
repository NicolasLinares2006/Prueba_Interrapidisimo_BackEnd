using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Inscripcion
    {
        public int Id { get; set; } 

        public int EventoId { get; set; }

        public int UsuarioId { get; set; }

    }
}
