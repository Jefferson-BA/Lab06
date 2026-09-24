USE NeptunoDB;
GO

/* ============================================================
   1. ALTER TABLES: Agregar campo de baja lógica 'Activo'
   ============================================================ */
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Productos') AND name = 'Activo')
    ALTER TABLE dbo.Productos ADD Activo BIT NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Categorias') AND name = 'Activo')
    ALTER TABLE dbo.Categorias ADD Activo BIT NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Proveedores') AND name = 'Activo')
    ALTER TABLE dbo.Proveedores ADD Activo BIT NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT 1;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Pedidos') AND name = 'Activo')
    ALTER TABLE dbo.Pedidos ADD Activo BIT NOT NULL CONSTRAINT DF_Pedidos_Activo DEFAULT 1;
GO

/* ============================================================
   2. PROCEDIMIENTOS ALMACENADOS - PRODUCTOS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_ListarProductos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProductoID, NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia, Activo
    FROM dbo.Productos
    WHERE Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertarProducto
    @NombreProducto NVARCHAR(60),
    @ProveedorID INT,
    @CategoriaID INT,
    @CantidadPorUnidad NVARCHAR(30),
    @PrecioUnidad DECIMAL(10,2),
    @UnidadesEnExistencia SMALLINT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Productos (NombreProducto, ProveedorID, CategoriaID, CantidadPorUnidad, PrecioUnidad, UnidadesEnExistencia, Activo)
    VALUES (@NombreProducto, @ProveedorID, @CategoriaID, @CantidadPorUnidad, @PrecioUnidad, @UnidadesEnExistencia, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarProducto
    @ProductoID INT,
    @NombreProducto NVARCHAR(60),
    @ProveedorID INT,
    @CategoriaID INT,
    @CantidadPorUnidad NVARCHAR(30),
    @PrecioUnidad DECIMAL(10,2),
    @UnidadesEnExistencia SMALLINT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET NombreProducto = @NombreProducto,
        ProveedorID = @ProveedorID,
        CategoriaID = @CategoriaID,
        CantidadPorUnidad = @CantidadPorUnidad,
        PrecioUnidad = @PrecioUnidad,
        UnidadesEnExistencia = @UnidadesEnExistencia
    WHERE ProductoID = @ProductoID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarProductoLogico
    @ProductoID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Productos
    SET Activo = 0
    WHERE ProductoID = @ProductoID;
END;
GO

/* ============================================================
   3. PROCEDIMIENTOS ALMACENADOS - CATEGORÍAS
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_ListarCategorias
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CategoriaID, NombreCategoria, Descripcion, Activo
    FROM dbo.Categorias
    WHERE Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertarCategoria
    @NombreCategoria NVARCHAR(30),
    @Descripcion NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Categorias (NombreCategoria, Descripcion, Activo)
    VALUES (@NombreCategoria, @Descripcion, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarCategoria
    @CategoriaID INT,
    @NombreCategoria NVARCHAR(30),
    @Descripcion NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Categorias
    SET NombreCategoria = @NombreCategoria,
        Descripcion = @Descripcion
    WHERE CategoriaID = @CategoriaID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarCategoriaLogico
    @CategoriaID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Categorias
    SET Activo = 0
    WHERE CategoriaID = @CategoriaID;
END;
GO

/* ============================================================
   4. PROCEDIMIENTOS ALMACENADOS - PROVEEDORES
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_ListarProveedores
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto, Ciudad, Telefono, Activo
    FROM dbo.Proveedores
    WHERE Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertarProveedor
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40),
    @CargoContacto NVARCHAR(40),
    @Ciudad NVARCHAR(30),
    @Telefono NVARCHAR(24)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Proveedores (CompaniaNombre, NombreContacto, CargoContacto, Ciudad, Telefono, Activo)
    VALUES (@CompaniaNombre, @NombreContacto, @CargoContacto, @Ciudad, @Telefono, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarProveedor
    @ProveedorID INT,
    @CompaniaNombre NVARCHAR(60),
    @NombreContacto NVARCHAR(40),
    @CargoContacto NVARCHAR(40),
    @Ciudad NVARCHAR(30),
    @Telefono NVARCHAR(24)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Proveedores
    SET CompaniaNombre = @CompaniaNombre,
        NombreContacto = @NombreContacto,
        CargoContacto = @CargoContacto,
        Ciudad = @Ciudad,
        Telefono = @Telefono
    WHERE ProveedorID = @ProveedorID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarProveedorLogico
    @ProveedorID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Proveedores
    SET Activo = 0
    WHERE ProveedorID = @ProveedorID;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_BuscarProveedores
    @NombreContacto NVARCHAR(40) = NULL,
    @Ciudad NVARCHAR(30) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ProveedorID, CompaniaNombre, NombreContacto, CargoContacto, Ciudad, Telefono, Activo
    FROM dbo.Proveedores
    WHERE Activo = 1
      AND (@NombreContacto IS NULL OR @NombreContacto = '' OR NombreContacto LIKE '%' + @NombreContacto + '%')
      AND (@Ciudad IS NULL OR @Ciudad = '' OR Ciudad LIKE '%' + @Ciudad + '%');
END;
GO

/* ============================================================
   5. PROCEDIMIENTOS ALMACENADOS - PEDIDOS Y REPORTES
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_ListarPedidos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT PedidoID, ClienteID, EmpleadoID, FechaPedido, FechaRequerida, FechaEnvio, Destinatario, CiudadDestino, Activo
    FROM dbo.Pedidos
    WHERE Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_InsertarPedido
    @ClienteID INT,
    @EmpleadoID INT,
    @FechaPedido DATE,
    @FechaRequerida DATE,
    @Destinatario NVARCHAR(60),
    @CiudadDestino NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.Pedidos (ClienteID, EmpleadoID, FechaPedido, FechaRequerida, Destinatario, CiudadDestino, Activo)
    VALUES (@ClienteID, @EmpleadoID, @FechaPedido, @FechaRequerida, @Destinatario, @CiudadDestino, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarPedido
    @PedidoID INT,
    @ClienteID INT,
    @EmpleadoID INT,
    @FechaPedido DATE,
    @FechaRequerida DATE,
    @Destinatario NVARCHAR(60),
    @CiudadDestino NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pedidos
    SET ClienteID = @ClienteID,
        EmpleadoID = @EmpleadoID,
        FechaPedido = @FechaPedido,
        FechaRequerida = @FechaRequerida,
        Destinatario = @Destinatario,
        CiudadDestino = @CiudadDestino
    WHERE PedidoID = @PedidoID AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_EliminarPedidoLogico
    @PedidoID INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Pedidos
    SET Activo = 0
    WHERE PedidoID = @PedidoID;
END;
GO

-- Reporte de detalles de pedidos por rango de fechas (excluyendo Pedidos Inactivos Activo = 0)
CREATE OR ALTER PROCEDURE dbo.sp_ListarDetallesPedidosPorFechas
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT 
        dp.PedidoID,
        p.FechaPedido,
        p.Destinatario,
        prod.NombreProducto,
        dp.PrecioUnidad,
        dp.Cantidad,
        dp.Descuento,
        (dp.PrecioUnidad * dp.Cantidad * (1 - dp.Descuento)) AS SubTotal
    FROM dbo.DetallePedidos dp
    INNER JOIN dbo.Pedidos p ON dp.PedidoID = p.PedidoID
    INNER JOIN dbo.Productos prod ON dp.ProductoID = prod.ProductoID
    WHERE p.Activo = 1
      AND p.FechaPedido BETWEEN @FechaInicio AND @FechaFin;
END;
GO