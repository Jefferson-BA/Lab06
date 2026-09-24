using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab06.Data.Repositories;
using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels
{
    public class CategoriasViewModel : ViewModelBase
    {
        private readonly CategoriaRepository _repo = new CategoriaRepository();
        private DataView _categoriasView;
        private DataRowView _selectedCategoria;

        private int _categoriaID;
        private string _nombreCategoria;
        private string _descripcion;

        public DataView CategoriasView
        {
            get => _categoriasView;
            set { _categoriasView = value; OnPropertyChanged(); }
        }

        public DataRowView SelectedCategoria
        {
            get => _selectedCategoria;
            set { _selectedCategoria = value; OnPropertyChanged(); CargarCampos(); }
        }

        public int CategoriaID { get => _categoriaID; set { _categoriaID = value; OnPropertyChanged(); } }
        public string NombreCategoria { get => _nombreCategoria; set { _nombreCategoria = value; OnPropertyChanged(); } }
        public string Descripcion { get => _descripcion; set { _descripcion = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public CategoriasViewModel()
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
            CategoriasView = dt.DefaultView;
        }

        private void CargarCampos()
        {
            if (SelectedCategoria == null) return;
            CategoriaID = Convert.ToInt32(SelectedCategoria["CategoriaID"]);
            NombreCategoria = SelectedCategoria["NombreCategoria"].ToString();
            Descripcion = SelectedCategoria["Descripcion"].ToString();
        }

        private void LimpiarCampos()
        {
            CategoriaID = 0;
            NombreCategoria = string.Empty;
            Descripcion = string.Empty;
            SelectedCategoria = null;
        }

        private async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(NombreCategoria)) return;

            if (CategoriaID == 0)
                await _repo.InsertarAsync(NombreCategoria, Descripcion);
            else
                await _repo.ActualizarAsync(CategoriaID, NombreCategoria, Descripcion);

            await CargarDatosAsync();
            LimpiarCampos();
            MessageBox.Show("Categoría guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task EliminarLogicoAsync()
        {
            if (CategoriaID <= 0) return;
            if (MessageBox.Show("¿Dar de baja esta categoría?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _repo.EliminarLogicoAsync(CategoriaID);
                await CargarDatosAsync();
                LimpiarCampos();
                MessageBox.Show("Categoría desactivada.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}