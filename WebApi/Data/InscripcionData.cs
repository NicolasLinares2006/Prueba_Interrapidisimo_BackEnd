using WebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Data
{
    public class InscripcionData
    {
        private readonly string conexion;

        public InscripcionData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<string>> InscribirUsuario(Inscripcion inscripcion)
        {
            var mensajes = new List<string>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                using (var cmd = new SqlCommand("InscribirseEnEvento", con))
                {
                    cmd.Parameters.AddWithValue("@pEventoId", inscripcion.EventoId);
                    cmd.Parameters.AddWithValue("@pUsuarioId", inscripcion.UsuarioId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        foreach (SqlError error in ex.Errors)
                        {
                            mensajes.Add(error.Message);
                        }
                    }
                }
            }

            if (mensajes.Count == 0)
            {
                mensajes.Add("Usuario inscrito exitosamente.");
            }

            return mensajes;
        }

    }
}
