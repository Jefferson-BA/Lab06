using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab06.Data.Repositories;
using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels
{
    public class ProductosViewModel : ViewModelBase
    {
        private readonly ProductoRepository _repo = new ProductoRepository();
        private DataView _productosView;
        private DataRowView _selectedProducto;

        private int _productoID;
        private string _nombreProducto;
        private int _proveedorID;
        private int _categoriaID;
        private string _cantidadPorUnidad;
        private decimal _precioUnidad;
        private short _unidadesEnExistencia;

        public DataView ProductosView
        {
            get => _productosView;
            set { _productosView = value; OnPropertyChanged(); }
        }

        public DataRowView SelectedProducto
        {
            get => _selectedProducto;
            set { _selectedProducto = value; OnPropertyChanged(); CargarCampos(); }
        }

        public int ProductoID { get => _productoID; set { _productoID = value; OnPropertyChanged(); } }
        public string NombreProducto { get => _nombreProducto; set { _nombreProducto = value; OnPropertyChanged(); } }
        public int ProveedorID { get => _proveedorID; set { _proveedorID = value; OnPropertyChanged(); } }
        public int CategoriaID { get => _categoriaID; set { _categoriaID = value; OnPropertyChanged(); } }
        public string CantidadPorUnidad { get => _cantidadPorUnidad; set { _cantidadPorUnidad = value; OnPropertyChanged(); } }
        public decimal PrecioUnidad { get => _precioUnidad; set { _precioUnidad = value; OnPropertyChanged(); } }
        public short UnidadesEnExistencia { get => _unidadesEnExistencia; set { _unidadesEnExistencia = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public ProductosViewModel()
        {
            CargarCommand = new RelayCommand(async _ => await CargarDatosAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync());
            EliminarCommand = new RelayCommand(async _ => await EliminarLogicoAsync());
            LimpiarCommand = new RelayCommand(_ => LimpiarCampos());
            _ = CargarDatosAsync();
        }

        public async Task CargarDatosAsync()
        {
            DataTable dt = await _repo.ListarDesconectadoAsync();
            ProductosView = dt.DefaultView;
        }

        private void CargarCampos()
        {
            if (SelectedProducto == null) return;
            ProductoID = Convert.ToInt32(SelectedProducto["ProductoID"]);
            NombreProducto = SelectedProducto["NombreProducto"].ToString();
            ProveedorID = Convert.ToInt32(SelectedProducto["ProveedorID"]);
            CategoriaID = Convert.ToInt32(SelectedProducto["CategoriaID"]);
            CantidadPorUnidad = SelectedProducto["CantidadPorUnidad"].ToString();
            PrecioUnidad = Convert.ToDecimal(SelectedProducto["PrecioUnidad"]);
            UnidadesEnExistencia = Convert.ToInt16(SelectedProducto["UnidadesEnExistencia"]);
        }

        private void LimpiarCampos()
        {
            ProductoID = 0;
            NombreProducto = string.Empty;
            ProveedorID = 0;
            CategoriaID = 0;
            CantidadPorUnidad = string.Empty;
            PrecioUnidad = 0;
            UnidadesEnExistencia = 0;
            SelectedProducto = null;
        }

        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreProducto))
            {
                MessageBox.Show("Ingrese un nombre de producto válido.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ProductoID == 0)
                await _repo.InsertarAsync(NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia);
            else
                await _repo.ActualizarAsync(ProductoID, NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia);

            await CargarDatosAsync();
            LimpiarCampos();
            MessageBox.Show("Operación realizada con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task EliminarLogicoAsync()
        {
            if (ProductoID <= 0) return;
            if (MessageBox.Show("¿Dar de baja este producto (Baja Lógica)?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _repo.EliminarLogicoAsync(ProductoID);
                await CargarDatosAsync();
                LimpiarCampos();
                MessageBox.Show("Producto dado de baja lógicamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}