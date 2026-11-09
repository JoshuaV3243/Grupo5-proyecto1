namespace SistemaVentasTienda.Core.Modelos
{
    /// <summary>Producto y cantidad que todavía no constituyen una venta confirmada.</summary>
    public class LineaCarrito
    {
        public int ProductoId { get; internal set; }
        public string Codigo { get; internal set; }
        public string Nombre { get; internal set; }
        public decimal PrecioUnitario { get; internal set; }
        public int Cantidad { get; internal set; }
        public decimal Subtotal { get { return PrecioUnitario * Cantidad; } }

        internal LineaCarrito Copiar()
        {
            return new LineaCarrito
            {
                ProductoId = ProductoId,
                Codigo = Codigo,
                Nombre = Nombre,
                PrecioUnitario = PrecioUnitario,
                Cantidad = Cantidad
            };
        }
    }
}
