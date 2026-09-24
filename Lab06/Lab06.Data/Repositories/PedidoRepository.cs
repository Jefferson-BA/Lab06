using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab06.Data.Repositories
{
    public class PedidoRepository
    {
        public async Task<DataTable> ListarDesconectadoAsync()
        {
            DataTable dt = new DataTable("Pedidos");
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ListarPedidos", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }

        public async Task<bool> InsertarAsync(int idCliente, int idEmpleado, DateTime fPedido, DateTime fReq, string dest, string ciudad)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_InsertarPedido", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClienteID", idCliente);
                cmd.Parameters.AddWithValue("@EmpleadoID", idEmpleado);
                cmd.Parameters.AddWithValue("@FechaPedido", fPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", fReq);
                cmd.Parameters.AddWithValue("@Destinatario", dest ?? "");
                cmd.Parameters.AddWithValue("@CiudadDestino", ciudad ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> ActualizarAsync(int idPedido, int idCliente, int idEmpleado, DateTime fPedido, DateTime fReq, string dest, string ciudad)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ActualizarPedido", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", idPedido);
                cmd.Parameters.AddWithValue("@ClienteID", idCliente);
                cmd.Parameters.AddWithValue("@EmpleadoID", idEmpleado);
                cmd.Parameters.AddWithValue("@FechaPedido", fPedido);
                cmd.Parameters.AddWithValue("@FechaRequerida", fReq);
                cmd.Parameters.AddWithValue("@Destinatario", dest ?? "");
                cmd.Parameters.AddWithValue("@CiudadDestino", ciudad ?? "");

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }

        public async Task<bool> EliminarLogicoAsync(int idPedido)
        {
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_EliminarPedidoLogico", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PedidoID", idPedido);

                await cn.OpenAsync();
                return (await cmd.ExecuteNonQueryAsync()) > 0;
            }
        }
    }
}