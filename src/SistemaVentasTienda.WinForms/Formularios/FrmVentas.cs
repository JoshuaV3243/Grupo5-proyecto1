using System;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Modelos;
using SistemaVentasTienda.Core.Servicios;
using SistemaVentasTienda.WinForms.Interfaz;

namespace SistemaVentasTienda.WinForms.Formularios
{
    public partial class FrmVentas : Form
    {
        private readonly ServicioProductos productos;
        private readonly ServicioVentas ventas;

        public FrmVentas()
        {
            InitializeComponent();
            Estilo.Aplicar(this);
            Estilo.PrepararTabla(dgvProductos);
            Estilo.Columna(dgvProductos, "Codigo", "Código", 80);
            Estilo.Columna(dgvProductos, "Nombre", "Producto", 150);
            Estilo.Columna(dgvProductos, "Precio", "Precio CRC", 90, "N2");
            Estilo.Columna(dgvProductos, "Existencias", "Existencias", 70, "N0");
            Estilo.PrepararTabla(dgvCarrito);
            Estilo.Columna(dgvCarrito, "Nombre", "Producto", 150);
            Estilo.Columna(dgvCarrito, "PrecioUnitario", "Precio CRC", 85, "N2");
            Estilo.Columna(dgvCarrito, "Cantidad", "Cantidad", 70, "N0");
            Estilo.Columna(dgvCarrito, "Subtotal", "Subtotal CRC", 100, "N2");
            nudCantidad.Maximum = Reglas.MaximoExistencias;
            nudCantidadCarrito.Maximum = Reglas.MaximoExistencias;
            nudRecibido.Maximum = Reglas.MontoMaximo;
            txtCliente.MaxLength = Reglas.LongitudCliente;
            dtpFecha.MinDate = Reglas.FechaMinima;
            dtpFecha.MaxDate = DateTime.Today;
            cboPago.Items.AddRange(new object[] { "Efectivo", "Tarjeta", "Transferencia" });
            cboPago.SelectedIndex = 0;
            Estilo.BotonPrincipal(btnConfirmar);
            Estilo.BotonPrincipal(btnAgregar);
            Estilo.BotonPeligro(btnVaciar);
        }

        public FrmVentas(ServicioProductos productos, ServicioVentas ventas) : this()
        {
            this.productos = productos;
            this.ventas = ventas;
            ActualizarProductos();
            ActualizarCarrito();
        }

        private void ActualizarProductos()
        {
            dgvProductos.DataSource = productos.Buscar(txtBuscar.Text);
            dgvProductos.ClearSelection();
        }

        private void ActualizarCarrito()
        {
            dgvCarrito.DataSource = ventas.ObtenerCarrito();
            btnConfirmar.Enabled = ventas.CalcularTotal() > 0m;
            ActualizarImportes();
        }

        private void ActualizarImportes()
        {
            if (ventas == null) return;
            decimal total = ventas.CalcularTotal();
            lblTotal.Text = "TOTAL CRC " + total.ToString("N2");
            bool esEfectivo = cboPago.SelectedIndex == (int)MedioPago.Efectivo;
            nudRecibido.Enabled = esEfectivo;
            if (!esEfectivo) nudRecibido.Value = total;
            decimal diferencia = nudRecibido.Value - total;
            lblCambio.Text = diferencia >= 0m ? "Cambio CRC " + diferencia.ToString("N2")
                : "Faltan CRC " + (-diferencia).ToString("N2");
            lblCambio.ForeColor = diferencia >= 0m ? Estilo.Verde : Estilo.Rojo;
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Seleccione un producto del inventario.", "Producto requerido");
                return;
            }
            Producto producto = (Producto)dgvProductos.SelectedRows[0].DataBoundItem;
            if (Mensajes.Ejecutar(this, () => ventas.AgregarProducto(producto.Id, Convert.ToInt32(nudCantidad.Value))))
            {
                ActualizarCarrito();
                nudCantidad.Value = 1m;
            }
        }

        private LineaCarrito SeleccionCarrito()
        {
            if (dgvCarrito.SelectedRows.Count == 0)
                throw new ValidacionException("Seleccione un producto del carrito.");
            return (LineaCarrito)dgvCarrito.SelectedRows[0].DataBoundItem;
        }

        private void BtnCambiarCantidad_Click(object sender, EventArgs e)
        {
            if (Mensajes.Ejecutar(this, () =>
                ventas.CambiarCantidad(SeleccionCarrito().ProductoId, Convert.ToInt32(nudCantidadCarrito.Value))))
                ActualizarCarrito();
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            if (Mensajes.Ejecutar(this, () => ventas.QuitarProducto(SeleccionCarrito().ProductoId))) ActualizarCarrito();
        }

        private void BtnVaciar_Click(object sender, EventArgs e)
        {
            if (ventas.ObtenerCarrito().Count == 0) return;
            if (!Mensajes.Confirmar(this, "¿Desea descartar todos los productos del carrito?")) return;
            ventas.LimpiarCarrito();
            ActualizarCarrito();
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (!Mensajes.Confirmar(this, "¿Desea registrar la venta por CRC " + ventas.CalcularTotal().ToString("N2") + "?")) return;
            Mensajes.Ejecutar(this, () =>
            {
                Venta venta = ventas.ConfirmarVenta(txtCliente.Text, dtpFecha.Value,
                    (MedioPago)cboPago.SelectedIndex, nudRecibido.Value);
                MessageBox.Show(this, "Venta " + venta.Numero + " registrada.\nTotal CRC " + venta.Total.ToString("N2")
                    + "\nCambio CRC " + venta.Cambio.ToString("N2"), "Venta completada", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Preparar una nueva venta después de confirmar la anterior.
                txtCliente.Text = "Consumidor final";
                dtpFecha.Value = DateTime.Today;
                nudRecibido.Value = 0m;
                nudCantidad.Value = 1m;
                ActualizarProductos();
                ActualizarCarrito();
            });
        }

        private void DgvCarrito_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCarrito.SelectedRows.Count > 0 && dgvCarrito.SelectedRows[0].DataBoundItem is LineaCarrito linea)
                nudCantidadCarrito.Value = linea.Cantidad;
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (productos != null) ActualizarProductos();
        }

        private void Pago_Cambiado(object sender, EventArgs e) { ActualizarImportes(); }
        private void BtnCerrar_Click(object sender, EventArgs e) { Close(); }

        private void FrmVentas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ventas == null || ventas.ObtenerCarrito().Count == 0) return;
            if (Mensajes.Confirmar(this, "La venta todavía no está confirmada.\n¿Desea descartar el carrito y volver al menú?"))
                ventas.LimpiarCarrito();
            else
                e.Cancel = true;
        }
    }
}
