using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab06.Data.Repositories;
using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels
{
    public class PedidosViewModel : ViewModelBase
    {
        private readonly PedidoRepository _repo = new PedidoRepository();
        private DataView _pedidosView;
        private DataRowView _selectedPedido;

        private int _pedidoID;
        private int _clienteID;
        private int _empleadoID;
        private DateTime _fechaPedido = DateTime.Now;
        private DateTime _fechaRequerida = DateTime.Now.AddDays(7);
        private string _destinatario;
        private string _ciudadDestino;

        public DataView PedidosView
        {
            get => _pedidosView;
            set { _pedidosView = value; OnPropertyChanged(); }
        }

        public DataRowView SelectedPedido
        {
            get => _selectedPedido;
            set { _selectedPedido = value; OnPropertyChanged(); CargarCampos(); }
        }

        public int PedidoID { get => _pedidoID; set { _pedidoID = value; OnPropertyChanged(); } }
        public int ClienteID { get => _clienteID; set { _clienteID = value; OnPropertyChanged(); } }
        public int EmpleadoID { get => _empleadoID; set { _empleadoID = value; OnPropertyChanged(); } }
        public DateTime FechaPedido { get => _fechaPedido; set { _fechaPedido = value; OnPropertyChanged(); } }
        public DateTime FechaRequerida { get => _fechaRequerida; set { _fechaRequerida = value; OnPropertyChanged(); } }
        public string Destinatario { get => _destinatario; set { _destinatario = value; OnPropertyChanged(); } }
        public string CiudadDestino { get => _ciudadDestino; set { _ciudadDestino = value; OnPropertyChanged(); } }

        public ICommand CargarCommand { get; }
        public ICommand GuardarCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand LimpiarCommand { get; }

        public PedidosViewModel()
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
            PedidosView = dt.DefaultView;
        }

        private void CargarCampos()
        {
            if (SelectedPedido == null) return;
            PedidoID = Convert.ToInt32(SelectedPedido["PedidoID"]);
            ClienteID = Convert.ToInt32(SelectedPedido["ClienteID"]);
            EmpleadoID = Convert.ToInt32(SelectedPedido["EmpleadoID"]);
            FechaPedido = Convert.ToDateTime(SelectedPedido["FechaPedido"]);
            FechaRequerida = Convert.ToDateTime(SelectedPedido["FechaRequerida"]);
            Destinatario = SelectedPedido["Destinatario"].ToString();
            CiudadDestino = SelectedPedido["CiudadDestino"].ToString();
        }

        private void LimpiarCampos()
        {
            PedidoID = 0;
            ClienteID = 0;
            EmpleadoID = 0;
            FechaPedido = DateTime.Now;
            FechaRequerida = DateTime.Now.AddDays(7);
            Destinatario = string.Empty;
            CiudadDestino = string.Empty;
            SelectedPedido = null;
        }

        private async Task GuardarAsync()
        {
            if (ClienteID <= 0 || EmpleadoID <= 0) return;

            if (PedidoID == 0)
                await _repo.InsertarAsync(ClienteID, EmpleadoID, FechaPedido, FechaRequerida, Destinatario, CiudadDestino);
            else
                await _repo.ActualizarAsync(PedidoID, ClienteID, EmpleadoID, FechaPedido, FechaRequerida, Destinatario, CiudadDestino);

            await CargarDatosAsync();
            LimpiarCampos();
            MessageBox.Show("Pedido registrado con éxito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task EliminarLogicoAsync()
        {
            if (PedidoID <= 0) return;
            if (MessageBox.Show("¿Dar de baja este pedido?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _repo.EliminarLogicoAsync(PedidoID);
                await CargarDatosAsync();
                LimpiarCampos();
                MessageBox.Show("Pedido dado de baja lógicamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}