# Lab06 - Sistema de Gestión Neptuno

Aplicación de escritorio desarrollada en **WPF** (.NET 10) con arquitectura en capas (MVVM + Repositorios) y acceso a datos mediante **ADO.NET** conectado a **SQL Server**.

---

## 🚀 Características y Módulos

- **Categorías**: Listado, registro, actualización y eliminación lógica.
- **Productos**: Mantenimiento completo de productos del catálogo.
- **Proveedores**: Búsqueda y gestión de proveedores.
- **Pedidos**: Administración y seguimiento de pedidos.
- **Reportes**: Consulta de detalle de pedidos filtrados por rango de fechas.

---

## 🛠️ Tecnologías Utilizadas

- **Lenguaje**: C# (.NET 10)
- **Interfaz Gráfica**: WPF (Windows Presentation Foundation)
- **Arquitectura**: Patrón MVVM (Model-View-ViewModel) + Patrón Repository
- **Acceso a Datos**: ADO.NET (`System.Data.SqlClient`)
- **Gestor de Base de Datos**: Microsoft SQL Server

---

## 🗄️ Base de Datos y Procedimientos Almacenados

- **Base de datos usada**: `NeptunoDB`
- **Procedimientos**: [Ver Script de Procedimientos Almacenados](./procedimientos.sql)

> 💡 *Nota: Puedes colocar tu script SQL en la raíz del proyecto con el nombre `procedimientos.sql` o ajustar la ruta del enlace según la ubicación de tu archivo.*

### Procedimientos Almacenados Implementados en el Sistema

| Módulo | Procedimiento Almacenado | Descripción |
| :--- | :--- | :--- |
| **Categorías** | `sp_ListarCategorias` | Consulta todas las categorías activas |
| | `sp_InsertarCategoria` | Registra una nueva categoría |
| | `sp_ActualizarCategoria` | Actualiza la información de una categoría |
| | `sp_EliminarCategoriaLogico` | Desactiva/elimina lógicamente una categoría |
| **Productos** | `sp_ListarProductos` | Lista los productos registrados |
| | `sp_InsertarProducto` | Crea un nuevo producto |
| | `sp_ActualizarProducto` | Modifica los datos de un producto |
| | `sp_EliminarProductoLogico` | Realiza la baja lógica de un producto |
| **Proveedores** | `sp_BuscarProveedores` | Búsqueda y listado de proveedores |
| | `sp_InsertarProveedor` | Inserta un nuevo proveedor |
| | `sp_ActualizarProveedor` | Modifica datos del proveedor |
| | `sp_EliminarProveedorLogico` | Baja lógica de un proveedor |
| **Pedidos** | `sp_ListarPedidos` | Consulta general de pedidos |
| | `sp_InsertarPedido` | Registra un nuevo pedido |
| | `sp_ActualizarPedido` | Actualiza información del pedido |
| | `sp_EliminarPedidoLogico` | Baja lógica de un pedido |
| **Reportes** | `sp_ListarDetallesPedidosPorFechas` | Detalle de pedidos entre rango de fechas |

---

## ⚙️ Configuración y Ejecución

1. **Restaurar / Crear la Base de Datos**:
   - Crear o importar la base de datos `NeptunoDB` en SQL Server.
   - Ejecutar el archivo de [procedimientos.sql](./procedimientos.sql).

2. **Configurar la Cadena de Conexión**:
   - Abrir el archivo `App.config` en el proyecto `Lab06`:
     ```xml
     <connectionStrings>
       <add name="NeptunoConnection"
            connectionString="Data Source=.;Initial Catalog=NeptunoDB;Integrated Security=True;TrustServerCertificate=True"
            providerName="System.Data.SqlClient" />
     </connectionStrings>
     ```
   - Modificar `Data Source` según el nombre de tu servidor o instancia local de SQL Server.

3. **Compilar y Ejecutar**:
   - Abrir la solución `Lab06.slnx` en Visual Studio 2022 o superior (con soporte para .NET 10).
   - Compilar la solución y presionar `F5` para iniciar.

---

## 📂 Estructura del Proyecto

```text
Lab06/
├── Lab06.Data/
│   ├── Models/          # Entidades de negocio
│   └── Repositories/    # Acceso a datos y consumo de SPs
├── Lab06.UI/
│   ├── Helpers/         # Clases de soporte (RelayCommand)
│   ├── ViewModels/      # Lógica de presentación (MVVM)
│   └── Views/           # Interfaces de usuario en XAML
├── App.config           # Cadena de conexión a NeptunoDB
├── procedimientos.sql   # Script SQL con los Stored Procedures
└── README.md
```
