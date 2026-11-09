namespace SistemaVentasTienda.Core.Modelos
{
    /// <summary>Artículo disponible para vender. Las reglas se aplican en ServicioProductos.</summary>
    public class Producto
    {
        public int Id { get; internal set; }
        public string Codigo { get; internal set; }
        public string Nombre { get; internal set; }
        public string Categoria { get; internal set; }
        public decimal Precio { get; internal set; }
        public int Existencias { get; internal set; }

        // Se entregan copias a los formularios para proteger el inventario original.
        internal Producto Copiar()
        {
            return new Producto
            {
                Id = Id,
                Codigo = Codigo,
                Nombre = Nombre,
                Categoria = Categoria,
                Precio = Precio,
                Existencias = Existencias
            };
        }
    }
}
