using System;
using System.Collections.Generic;
using SistemaVentasTienda.Core.Datos;
using SistemaVentasTienda.Core.Modelos;

namespace SistemaVentasTienda.Core.Servicios
{
    /// <summary>Controla un carrito y registra sus ventas en el almacén compartido.</summary>
    public class ServicioVentas
    {
        private readonly AlmacenMemoria almacen;
        private readonly List<LineaCarrito> carrito = new List<LineaCarrito>();

        public ServicioVentas(AlmacenMemoria almacen)
        {
            this.almacen = almacen ?? throw new ArgumentNullException(nameof(almacen));
        }

        public List<LineaCarrito> ObtenerCarrito()
        {
            List<LineaCarrito> copia = new List<LineaCarrito>();
            foreach (LineaCarrito linea in carrito) copia.Add(linea.Copiar());
            return copia;
        }

        public decimal CalcularTotal()
        {
            decimal total = 0m;
            foreach (LineaCarrito linea in carrito) total += linea.Subtotal;
            return total;
        }

        /// <summary>Repetir un producto suma su cantidad sin crear otra fila.</summary>
        public void AgregarProducto(int productoId, int cantidad)
        {
            Producto producto = EncontrarProducto(productoId);
            ValidarCantidad(cantidad);
            LineaCarrito existente = BuscarLinea(productoId);
            int cantidadAnterior = existente == null ? 0 : existente.Cantidad;
            if (cantidad > producto.Existencias - cantidadAnterior)
                throw new ValidacionException("La cantidad solicitada supera las existencias disponibles.");

            if (existente != null)
            {
                if (existente.PrecioUnitario != producto.Precio)
                    throw new ValidacionException("Cambió el precio. Quite el producto del carrito y agréguelo de nuevo.");
                existente.Cantidad += cantidad;
            }
            else
            {
                if (carrito.Count >= Reglas.MaximoLineas)
                    throw new ValidacionException("Una venta permite hasta " + Reglas.MaximoLineas + " productos distintos.");
                carrito.Add(new LineaCarrito { ProductoId = producto.Id, Codigo = producto.Codigo,
                    Nombre = producto.Nombre, PrecioUnitario = producto.Precio, Cantidad = cantidad });
            }
        }

        public void CambiarCantidad(int productoId, int cantidad)
        {
            ValidarCantidad(cantidad);
            Producto producto = EncontrarProducto(productoId);
            LineaCarrito linea = BuscarLinea(productoId);
            if (linea == null) throw new ValidacionException("Seleccione un producto del carrito.");
            if (cantidad > producto.Existencias)
                throw new ValidacionException("No hay existencias suficientes para esa cantidad.");
            linea.Cantidad = cantidad;
        }

        public void QuitarProducto(int productoId)
        {
            LineaCarrito linea = BuscarLinea(productoId);
            if (linea == null) throw new ValidacionException("Seleccione un producto del carrito.");
            carrito.Remove(linea);
        }

        public void LimpiarCarrito()
        {
            // Reservar o descartar un carrito no modifica las existencias.
            carrito.Clear();
        }

        /// <summary>
        /// Primero valida toda la operación. Solo entonces descuenta inventario,
        /// guarda la venta y vacía el carrito; no quedan descuentos parciales.
        /// </summary>
        public Venta ConfirmarVenta(string cliente, DateTime fecha, MedioPago medioPago, decimal montoRecibido)
        {
            if (carrito.Count == 0) throw new ValidacionException("Agregue al menos un producto a la venta.");
            string clienteLimpio = Reglas.TextoObligatorio(cliente, "cliente", Reglas.LongitudCliente);
            if (fecha.Date < Reglas.FechaMinima || fecha.Date > DateTime.Today)
                throw new ValidacionException("La fecha de venta debe estar entre el 01/01/2000 y hoy.");
            if (!Enum.IsDefined(typeof(MedioPago), medioPago))
                throw new ValidacionException("Seleccione un medio de pago válido.");

            decimal total = CalcularTotal();
            Reglas.ValidarDinero(montoRecibido, "El monto recibido", false);
            if (medioPago == MedioPago.Efectivo && montoRecibido < total)
                throw new ValidacionException("El efectivo recibido no alcanza para pagar la venta.");
            if (medioPago != MedioPago.Efectivo && montoRecibido != total)
                throw new ValidacionException("En tarjeta o transferencia el importe registrado debe ser igual al total.");

            List<DetalleVenta> detalles = new List<DetalleVenta>();
            foreach (LineaCarrito linea in carrito)
            {
                Producto producto = EncontrarProducto(linea.ProductoId);
                if (producto.Existencias < linea.Cantidad)
                    throw new ValidacionException("Cambió el inventario de " + linea.Nombre + ". Revise su cantidad.");
                if (producto.Precio != linea.PrecioUnitario || producto.Codigo != linea.Codigo || producto.Nombre != linea.Nombre)
                    throw new ValidacionException("Cambió el producto " + linea.Nombre + ". Quítelo y agréguelo de nuevo.");
                detalles.Add(new DetalleVenta(linea));
            }

            Venta venta = new Venta(almacen.SiguienteVentaId, fecha, clienteLimpio, medioPago, montoRecibido, detalles);
            foreach (LineaCarrito linea in carrito)
            {
                EncontrarProducto(linea.ProductoId).Existencias -= linea.Cantidad;
            }
            almacen.Ventas.Add(venta);
            almacen.SiguienteVentaId++;
            carrito.Clear();
            return venta;
        }

        /// <summary>Incluye ambos extremos del período; ordena primero la venta más reciente.</summary>
        public List<Venta> BuscarVentas(DateTime desde, DateTime hasta, string texto)
        {
            Reglas.ValidarPeriodo(desde, hasta);
            string filtro = (texto ?? "").Trim();
            List<Venta> resultado = new List<Venta>();
            // El ciclo for recorre hacia atrás para conservar el orden de registro dentro del día.
            for (int i = almacen.Ventas.Count - 1; i >= 0; i--)
            {
                Venta venta = almacen.Ventas[i];
                if (venta.Fecha >= desde.Date && venta.Fecha <= hasta.Date
                    && (venta.Numero.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                        || venta.Cliente.Contains(filtro, StringComparison.OrdinalIgnoreCase)))
                {
                    resultado.Add(venta);
                }
            }
            resultado.Sort((a, b) =>
            {
                int porFecha = b.Fecha.CompareTo(a.Fecha);
                return porFecha == 0 ? b.Id.CompareTo(a.Id) : porFecha;
            });
            return resultado;
        }

        private Producto EncontrarProducto(int id)
        {
            foreach (Producto producto in almacen.Productos)
                if (producto.Id == id) return producto;
            throw new ValidacionException("El producto ya no existe en el inventario.");
        }

        private LineaCarrito BuscarLinea(int productoId)
        {
            foreach (LineaCarrito linea in carrito)
                if (linea.ProductoId == productoId) return linea;
            return null;
        }

        private static void ValidarCantidad(int cantidad)
        {
            if (cantidad <= 0 || cantidad > Reglas.MaximoExistencias)
                throw new ValidacionException("La cantidad debe estar entre 1 y " + Reglas.MaximoExistencias + ".");
        }
    }
}
