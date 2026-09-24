namespace Lab06.Data.Models
{
    public class Proveedor
    {
        public int ProveedorID { get; set; }
        public string CompaniaNombre { get; set; }
        public string NombreContacto { get; set; }
        public string CargoContacto { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public bool Activo { get; set; }
    }
}