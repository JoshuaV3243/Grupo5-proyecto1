using System;
using System.Collections.Generic;

namespace SistemaVentasTienda.Core.Modelos
{
    /// <summary>Venta confirmada; sus datos y detalles no pueden editarse desde la interfaz.</summary>
    public class Venta
    {
        public int Id { get; private set; }
        public string Numero { get { return "V-" + Id.ToString("D5"); } }
        public DateTime Fecha { get; private set; }
        public string Cliente { get; private set; }
        public MedioPago MedioPago { get; private set; }
        public string FormaPago { get { return DescripcionPago.Obtener(MedioPago); } }
        public decimal Total { get; private set; }
        public decimal MontoRecibido { get; private set; }
        public decimal Cambio { get { return MontoRecibido - Total; } }
        public IReadOnlyList<DetalleVenta> Detalles { get; private set; }

        internal Venta(int id, DateTime fecha, string cliente, MedioPago medioPago,
            decimal montoRecibido, List<DetalleVenta> detalles)
        {
            Id = id;
            Fecha = fecha.Date;
            Cliente = cliente;
            MedioPago = medioPago;
            MontoRecibido = montoRecibido;
            Detalles = new List<DetalleVenta>(detalles).AsReadOnly();

            decimal total = 0m;
            foreach (DetalleVenta detalle in detalles)
            {
                total += detalle.Subtotal;
            }
            Total = total;
        }
    }
}
