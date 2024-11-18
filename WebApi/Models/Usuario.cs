using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Usuario
    {
        public int Id { get; set; } // Clave primaria con autoincremento
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Contraseña { get; set; }
    }
}
