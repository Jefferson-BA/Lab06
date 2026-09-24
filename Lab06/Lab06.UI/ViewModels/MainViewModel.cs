using Lab06.UI.Helpers;

namespace Lab06.UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ProductosViewModel ProductosVM { get; } = new ProductosViewModel();
    public CategoriasViewModel CategoriasVM { get; } = new CategoriasViewModel();
    public ProveedoresViewModel ProveedoresVM { get; } = new ProveedoresViewModel();
    public PedidosViewModel PedidosVM { get; } = new PedidosViewModel();
    public ReportesViewModel ReportesVM { get; } = new ReportesViewModel();
}