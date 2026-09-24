using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;
using Lab06.Data.Repositories;
using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels
{
    public class ReportesViewModel : ViewModelBase
    {
        private readonly ReporteRepository _repo = new ReporteRepository();
        private DataView _reporteView;
        private DateTime _fechaInicio = new DateTime(2026, 1, 1);
        private DateTime _fechaFin = DateTime.Now;

        public DataView ReporteView
        {
            get => _reporteView;
            set { _reporteView = value; OnPropertyChanged(); }
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set { _fechaInicio = value; OnPropertyChanged(); }
        }

        public DateTime FechaFin
        {
            get => _fechaFin;
            set { _fechaFin = value; OnPropertyChanged(); }
        }

        public ICommand GenerarReporteCommand { get; }

        public ReportesViewModel()
        {
            GenerarReporteCommand = new RelayCommand(async _ => await CargarReporteAsync());
            _ = CargarReporteAsync();
        }

        public async Task CargarReporteAsync()
        {
            DataTable dt = await _repo.ObtenerReporteDetallesPorFechasAsync(FechaInicio, FechaFin);
            ReporteView = dt.DefaultView;
        }
    }
}