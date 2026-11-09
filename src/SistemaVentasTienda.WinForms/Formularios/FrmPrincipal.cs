using System;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Modelos;
using SistemaVentasTienda.Core.Servicios;
using SistemaVentasTienda.WinForms.Interfaz;

namespace SistemaVentasTienda.WinForms.Formularios
{
    public partial class FrmPrincipal : Form
    {
        private readonly ServicioProductos productos;
        private readonly ServicioVentas ventas;

        // Constructor sin parámetros para que Visual Studio pueda abrir el diseñador.
        public FrmPrincipal()
        {
            InitializeComponent();
            Estilo.Aplicar(this);
            Estilo.BotonPrincipal(btnNuevaVenta);
        }

        public FrmPrincipal(ServicioProductos productos, ServicioVentas ventas) : this()
        {
            this.productos = productos;
            this.ventas = ventas;
            ActualizarResumen();
        }

        private void BtnProductos_Click(object sender, EventArgs e)
        {
            using (FrmProductos formulario = new FrmProductos(productos)) formulario.ShowDialog(this);
            ActualizarResumen();
        }

        private void BtnNuevaVenta_Click(object sender, EventArgs e)
        {
            using (FrmVentas formulario = new FrmVentas(productos, ventas)) formulario.ShowDialog(this);
            ActualizarResumen();
        }

        private void BtnHistorial_Click(object sender, EventArgs e)
        {
            using (FrmHistorial formulario = new FrmHistorial(ventas)) formulario.ShowDialog(this);
        }

        private void ActualizarResumen()
        {
            long unidades = 0;
            var inventario = productos.Buscar("");
            foreach (Producto producto in inventario) unidades += producto.Existencias;
            var ventasHoy = ventas.BuscarVentas(DateTime.Today, DateTime.Today, "");
            decimal totalHoy = 0m;
            foreach (Venta venta in ventasHoy) totalHoy += venta.Total;
            lblProductos.Text = inventario.Count + " productos\n" + unidades + " unidades en inventario";
            lblVentas.Text = ventasHoy.Count + " ventas de hoy\nCRC " + totalHoy.ToString("N2");
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            // El aviso corresponde al almacenamiento en memoria solicitado para este avance.
            if (productos != null && !Mensajes.Confirmar(this,
                "Al salir se perderán los productos y las ventas de esta sesión.\n¿Desea cerrar el sistema?"))
                e.Cancel = true;
        }
    }
}
