using WebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Data
{
    public class UsuarioData
    {
        private readonly string conexion;

        public UsuarioData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Usuario>> ListaUsuarios()
        {
            List<Usuario> listaUsuario = new List<Usuario>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("ObtenerUsuarios", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        listaUsuario.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString()
                        });

                    }
                }

                return listaUsuario;
            }
        }

        public async Task<Usuario> Usuario(int id)
        {
            Usuario usuario = new Usuario();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("ObtenerUsuario", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString()
                        };
                    }
                }
                return usuario;
            }
        }
    }
}
