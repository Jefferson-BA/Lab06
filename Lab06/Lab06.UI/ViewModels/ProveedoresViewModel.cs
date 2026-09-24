using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab06.Data.Repositories;
using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels
{
    public class ProveedoresViewModel : ViewModelBase
    {
        private readonly ProveedorRepository _repo = new ProveedorRepository();
        private DataView _proveedoresView;
        private DataRowView _selectedProveedor;

        private int _proveedorID;
        private string _companiaNombre;
        private string _nombreContacto;
        private string _cargoContacto;
        private string _ciudad;
        private string _telefono;

        private string _filtroContacto;
        private string _filtroCiudad;

        public DataView ProveedoresView
        {
            get => _proveedoresView;
            set { _proveedoresView = value; OnPropertyChanged(); }
        }

        public DataRowView SelectedProveedor
        {
            get => _selectedProveedor;
            set { _selectedProveedor = value; OnPropertyChanged(); CargarCampos(); }
        }

        public int ProveedorID { get => _proveedorID; set { _proveedorID = value; OnPropertyChanged(); } }
        public string CompaniaNombre { get => _companiaNombre; set { _companiaNombre = value; OnPropertyChanged(); } }
        public string NombreContacto { get => _nombreContacto; set { _nombreContacto = value; OnPropertyChanged(); } }
        public string CargoContacto { get => _cargoContacto; set { _cargoContacto = value; OnPropertyChanged(); } }
        public string Ciudad { get => _ciudad; set { _ciudad = value; OnPropertyChanged(); } }
        public string Telefono { get => _telefono; set { _telefono = value; OnPropertyChanged(); } }

        public string FiltroContacto { get => _filtroContacto; set { _filtroContacto = value; OnPropertyChanged(); } }
        public string FiltroCiudad { get => _filtroCiudad; set { _filtroCiudad = value; OnPropertyChanged(); } }

        public ICommand BuscarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public ProveedoresViewModel()
        {
            BuscarCommand = new RelayCommand(async _ => await BuscarAsync());
            GuardarCommand = new RelayCommand(async _ => await GuardarAsync());
            EliminarCommand = new RelayCommand(async _ => await EliminarLogicoAsync());
            LimpiarCommand = new RelayCommand(_ => LimpiarCampos());
            _ = BuscarAsync();
        }

        public async Task BuscarAsync()
        {
            DataTable dt = await _repo.BuscarDesconectadoAsync(FiltroContacto, FiltroCiudad);
            ProveedoresView = dt.DefaultView;
        }

        private void CargarCampos()
        {
            if (SelectedProveedor == null) return;
            ProveedorID = Convert.ToInt32(SelectedProveedor["ProveedorID"]);
            CompaniaNombre = SelectedProveedor["CompaniaNombre"].ToString();
            NombreContacto = SelectedProveedor["NombreContacto"].ToString();
            CargoContacto = SelectedProveedor["CargoContacto"].ToString();
            Ciudad = SelectedProveedor["Ciudad"].ToString();
            Telefono = SelectedProveedor["Telefono"].ToString();
        }

        private void LimpiarCampos()
        {
            ProveedorID = 0;
            CompaniaNombre = string.Empty;
            NombreContacto = string.Empty;
            CargoContacto = string.Empty;
            Ciudad = string.Empty;
            Telefono = string.Empty;
            SelectedProveedor = null;
        }

        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(CompaniaNombre)) return;

            if (ProveedorID == 0)
                await _repo.InsertarAsync(CompaniaNombre, NombreContacto, CargoContacto, Ciudad, Telefono);
            else
                await _repo.ActualizarAsync(ProveedorID, CompaniaNombre, NombreContacto, CargoContacto, Ciudad, Telefono);

            await BuscarAsync();
            LimpiarCampos();
            MessageBox.Show("Proveedor guardado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task EliminarLogicoAsync()
        {
            if (ProveedorID <= 0) return;
            if (MessageBox.Show("¿Dar de baja a este proveedor?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _repo.EliminarLogicoAsync(ProveedorID);
                await BuscarAsync();
                LimpiarCampos();
                MessageBox.Show("Proveedor dado de baja lógicamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}