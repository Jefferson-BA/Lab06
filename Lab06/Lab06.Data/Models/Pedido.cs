using System;

namespace Lab06.Data.Models
{
    public class Pedido
    {
        public int PedidoID { get; set; }
        public int ClienteID { get; set; }
        public int EmpleadoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaRequerida { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Destinatario { get; set; }
        public string CiudadDestino { get; set; }
        public bool Activo { get; set; }
    }
}