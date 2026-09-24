using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lab06.Data.Repositories
{
    public class ReporteRepository
    {
        public async Task<DataTable> ObtenerReporteDetallesPorFechasAsync(DateTime inicio, DateTime fin)
        {
            DataTable dt = new DataTable("DetallePedidosReporte");
            using (var cn = Conexion.ObtenerConexion())
            using (var cmd = new SqlCommand("sp_ListarDetallesPedidosPorFechas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", inicio);
                cmd.Parameters.AddWithValue("@FechaFin", fin);

                using (var da = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => da.Fill(dt));
                }
            }
            return dt;
        }
    }
}