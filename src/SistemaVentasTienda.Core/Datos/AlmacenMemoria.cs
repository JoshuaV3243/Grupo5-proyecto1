using System.Collections.Generic;
using SistemaVentasTienda.Core.Modelos;

namespace SistemaVentasTienda.Core.Datos
{
    /// <summary>
    /// Almacenamiento compartido de una sesión. No utiliza archivos ni base de datos.
    /// Solo los servicios del mismo proyecto pueden modificar estas listas.
    /// </summary>
    public class AlmacenMemoria
    {
        internal List<Producto> Productos { get; } = new List<Producto>();
        internal List<Venta> Ventas { get; } = new List<Venta>();
        internal int SiguienteProductoId { get; set; } = 1;
        internal int SiguienteVentaId { get; set; } = 1;
    }
}
