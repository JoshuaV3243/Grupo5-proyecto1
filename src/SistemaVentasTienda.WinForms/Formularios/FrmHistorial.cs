using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Modelos;
using SistemaVentasTienda.Core.Servicios;
using SistemaVentasTienda.WinForms.Interfaz;

namespace SistemaVentasTienda.WinForms.Formularios
{
    public partial class FrmHistorial : Form
    {
        private readonly ServicioVentas ventas;

        public FrmHistorial()
        {
            InitializeComponent();
            Estilo.Aplicar(this);
            Estilo.PrepararTabla(dgvVentas);
            Estilo.Columna(dgvVentas, "Numero", "Número", 80);
            Estilo.Columna(dgvVentas, "Fecha", "Fecha", 90, "dd/MM/yyyy");
            Estilo.Columna(dgvVentas, "Cliente", "Cliente", 150);
            Estilo.Columna(dgvVentas, "FormaPago", "Medio de pago", 100);
            Estilo.Columna(dgvVentas, "Total", "Total CRC", 95, "N2");
            Estilo.PrepararTabla(dgvDetalles);
            Estilo.Columna(dgvDetalles, "Codigo", "Código", 80);
            Estilo.Columna(dgvDetalles, "Nombre", "Producto", 150);
            Estilo.Columna(dgvDetalles, "PrecioUnitario", "Precio CRC", 95, "N2");
            Estilo.Columna(dgvDetalles, "Cantidad", "Cantidad", 70, "N0");
            Estilo.Columna(dgvDetalles, "Subtotal", "Subtotal CRC", 100, "N2");
            dtpDesde.MinDate = dtpHasta.MinDate = Reglas.FechaMinima;
            dtpDesde.MaxDate = dtpHasta.MaxDate = DateTime.Today;
            Estilo.BotonPrincipal(btnBuscar);
        }

        public FrmHistorial(ServicioVentas ventas) : this()
        {
            this.ventas = ventas;
            Buscar();
        }

        private void Buscar()
        {
            Mensajes.Ejecutar(this, () =>
            {
                List<Venta> lista = ventas.BuscarVentas(dtpDesde.Value, dtpHasta.Value, txtBuscar.Text);
                dgvVentas.DataSource = lista;
                decimal total = 0m;
                foreach (Venta venta in lista) total += venta.Total;
                lblResumen.Text = lista.Count + " ventas encontradas | Total CRC " + total.ToString("N2");
                if (lista.Count == 0)
                {
                    dgvDetalles.DataSource = new List<DetalleVenta>();
                    lblDetalle.Text = "No hay ventas para los filtros seleccionados.";
                }
                else
                {
                    MostrarDetalle();
                }
            });
        }

        private void MostrarDetalle()
        {
            if (dgvVentas.CurrentRow == null) return;
            Venta venta = dgvVentas.CurrentRow.DataBoundItem as Venta;
            if (venta == null) return;
            dgvDetalles.DataSource = new List<DetalleVenta>(venta.Detalles);
            lblDetalle.Text = venta.Numero + " | " + venta.Cliente + " | " + venta.FormaPago
                + " | Recibido CRC " + venta.MontoRecibido.ToString("N2") + " | Cambio CRC " + venta.Cambio.ToString("N2");
        }

        private void BtnBuscar_Click(object sender, EventArgs e) { Buscar(); }

        private void BtnTodas_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = Reglas.FechaMinima;
            dtpHasta.Value = DateTime.Today;
            txtBuscar.Clear();
            Buscar();
        }

        private void DgvVentas_SelectionChanged(object sender, EventArgs e) { MostrarDetalle(); }
        private void BtnCerrar_Click(object sender, EventArgs e) { Close(); }
    }
}
