using System.Data.SqlClient;

namespace Lab06.Data
{
    public static class Conexion
    {
        public static string Cadena =>
            @"Server=LAPTOP-T466KP2I\SQLEXPRESS;Database=NeptunoDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(Cadena);
        }
    }
}