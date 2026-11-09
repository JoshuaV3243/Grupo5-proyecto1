namespace SistemaVentasTienda.Core.Modelos
{
    /// <summary>Copia histórica del producto y precio que se vendieron.</summary>
    public class DetalleVenta
    {
        public int ProductoId { get; private set; }
        public string Codigo { get; private set; }
        public string Nombre { get; private set; }
        public decimal PrecioUnitario { get; private set; }
        public int Cantidad { get; private set; }
        public decimal Subtotal { get { return PrecioUnitario * Cantidad; } }

        internal DetalleVenta(LineaCarrito linea)
        {
            ProductoId = linea.ProductoId;
            Codigo = linea.Codigo;
            Nombre = linea.Nombre;
            PrecioUnitario = linea.PrecioUnitario;
            Cantidad = linea.Cantidad;
        }
    }
}
