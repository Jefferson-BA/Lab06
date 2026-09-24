using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab06.Data.Repositories
{
    public class ProductoRepository
    {
        public async Task<DataTable> ListarDesconectadoAsync()
        {
            DataTable dt = new DataTable("Productos");
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ListarProductos", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }

        public async Task<bool> InsertarAsync(string nombre, int idProv, int idCat, string cantUnidad, decimal precio, short stock)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_InsertarProducto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProducto", nombre);
                cmd.Parameters.AddWithValue("@ProveedorID", idProv);
                cmd.Parameters.AddWithValue("@CategoriaID", idCat);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", cantUnidad ?? "");
                cmd.Parameters.AddWithValue("@PrecioUnidad", precio);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", stock);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> ActualizarAsync(int id, string nombre, int idProv, int idCat, string cantUnidad, decimal precio, short stock)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ActualizarProducto", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", id);
                cmd.Parameters.AddWithValue("@NombreProducto", nombre);
                cmd.Parameters.AddWithValue("@ProveedorID", idProv);
                cmd.Parameters.AddWithValue("@CategoriaID", idCat);
                cmd.Parameters.AddWithValue("@CantidadPorUnidad", cantUnidad ?? "");
                cmd.Parameters.AddWithValue("@PrecioUnidad", precio);
                cmd.Parameters.AddWithValue("@UnidadesEnExistencia", stock);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> EliminarLogicoAsync(int id)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_EliminarProductoLogico", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductoID", id);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }
    }
}