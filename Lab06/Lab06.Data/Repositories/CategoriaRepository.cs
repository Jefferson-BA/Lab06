using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab06.Data.Repositories
{
    public class CategoriaRepository
    {
        public async Task<DataTable> ListarDesconectadoAsync()
        {
            DataTable dt = new DataTable("Categorias");
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ListarCategorias", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }

        public async Task<bool> InsertarAsync(string nombre, string descripcion)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_InsertarCategoria", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreCategoria", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> ActualizarAsync(int id, string nombre, string descripcion)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ActualizarCategoria", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", id);
                cmd.Parameters.AddWithValue("@NombreCategoria", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_EliminarCategoriaLogico", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoriaID", id);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }
    }
}