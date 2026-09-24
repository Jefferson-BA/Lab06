using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab06.Data.Repositories
{
    public class ProveedorRepository
    {
        public async Task<DataTable> BuscarDesconectadoAsync(string contacto, string ciudad)
        {
            DataTable dt = new DataTable("Proveedores");
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_BuscarProveedores", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreContacto", (object)contacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Ciudad", (object)ciudad ?? DBNull.Value);

                using (var da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }

        public async Task<bool> InsertarAsync(string compania, string contacto, string cargo, string ciudad, string telefono)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_InsertarProveedor", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompaniaNombre", compania);
                cmd.Parameters.AddWithValue("@NombreContacto", contacto ?? "");
                cmd.Parameters.AddWithValue("@CargoContacto", cargo ?? "");
                cmd.Parameters.AddWithValue("@Ciudad", ciudad ?? "");
                cmd.Parameters.AddWithValue("@Telefono", telefono ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> ActualizarAsync(int id, string compania, string contacto, string cargo, string ciudad, string telefono)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ActualizarProveedor", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", id);
                cmd.Parameters.AddWithValue("@CompaniaNombre", compania);
                cmd.Parameters.AddWithValue("@NombreContacto", contacto ?? "");
                cmd.Parameters.AddWithValue("@CargoContacto", cargo ?? "");
                cmd.Parameters.AddWithValue("@Ciudad", ciudad ?? "");
                cmd.Parameters.AddWithValue("@Telefono", telefono ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_EliminarProveedorLogico", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProveedorID", id);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }
    }
}