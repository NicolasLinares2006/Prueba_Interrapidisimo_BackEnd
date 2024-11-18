using WebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.Data
{
    public class EventoData
    {
        private readonly string conexion;

        public EventoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<Evento>> ListaEventos()
        {
            List<Evento> listaEvento = new List<Evento>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("ObtenerEventos", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync()) {

                        listaEvento.Add(new Evento { 
                            Id = Convert.ToInt32(reader["Id"]), 
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                            Ubicacion = reader["Ubicacion"].ToString(),
                            CapacidadMaxima = Convert.ToInt32(reader["CapacidadMaxima"]),
                            Asistentes = Convert.ToInt32(reader["AsistentesRegistrados"]),
                            Usuario = new Usuario
                            {
                                Id = Convert.ToInt32(reader["UsuarioId"]),
                                Nombre = reader["UsuarioNombre"].ToString()
                            }
                        });

                    }
                }

                return listaEvento;
            }
        }

        public async Task<Evento> Evento(int id)
        {
            Evento evento = new Evento();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("ObtenerEvento", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        evento = new Evento
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                            Ubicacion = reader["Ubicacion"].ToString(),
                            CapacidadMaxima = Convert.ToInt32(reader["CapacidadMaxima"]),
                            Asistentes = Convert.ToInt32(reader["AsistentesRegistrados"]),
                            Usuario = new Usuario
                            {
                                Id = Convert.ToInt32(reader["UsuarioId"]),
                                Nombre = reader["UsuarioNombre"].ToString()
                            }
                        };
                    }
                }
                return evento;
            }
        }

        public async Task<bool> CrearEvento(Evento evento)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("CrearEvento", con);
                cmd.Parameters.AddWithValue("@pNombre", evento.Nombre);
                cmd.Parameters.AddWithValue("@pDescripcion", evento.Descripcion);
                cmd.Parameters.AddWithValue("@pFechaHora", evento.FechaHora);
                cmd.Parameters.AddWithValue("@pUbicacion", evento.Ubicacion);
                cmd.Parameters.AddWithValue("@pCapacidadMaxima", evento.CapacidadMaxima);
                cmd.Parameters.AddWithValue("@pUsuarioId", evento.UsuarioId);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true: false;    
                }
                catch
                {
                    respuesta = false;
                }

                return respuesta;
            }
        }

        public async Task<bool> EditarEvento(Evento evento)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("EditarEvento", con);
                cmd.Parameters.AddWithValue("@pEventoId ", evento.Id);
                cmd.Parameters.AddWithValue("@pFechaHora", evento.FechaHora);
                cmd.Parameters.AddWithValue("@pUbicacion", evento.Ubicacion);
                cmd.Parameters.AddWithValue("@pCapacidadMaxima", evento.CapacidadMaxima);
                cmd.Parameters.AddWithValue("@pUsuarioId", evento.Usuario.Id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }

                return respuesta;
            }
        }

        public async Task<bool> EliminarEvento(int id)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("EliminarEvento", con);
                cmd.Parameters.AddWithValue("@pEventoId ", id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }

                return respuesta;
            }
        }
    }
}
