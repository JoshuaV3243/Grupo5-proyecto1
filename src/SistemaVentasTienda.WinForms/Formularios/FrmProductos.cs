using System;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Modelos;
using SistemaVentasTienda.Core.Servicios;
using SistemaVentasTienda.WinForms.Interfaz;

namespace SistemaVentasTienda.WinForms.Formularios
{
    public partial class FrmProductos : Form
    {
        private readonly ServicioProductos productos;
        private int productoEnEdicion;

        public FrmProductos()
        {
            InitializeComponent();
            Estilo.Aplicar(this);
            Estilo.PrepararTabla(dgvProductos);
            Estilo.Columna(dgvProductos, "Codigo", "Código", 80);
            Estilo.Columna(dgvProductos, "Nombre", "Nombre", 160);
            Estilo.Columna(dgvProductos, "Categoria", "Categoría", 100);
            Estilo.Columna(dgvProductos, "Precio", "Precio CRC", 85, "N2");
            Estilo.Columna(dgvProductos, "Existencias", "Existencias", 75, "N0");
            Estilo.BotonPrincipal(btnGuardar);
            Estilo.BotonPeligro(btnEliminar);
            txtCodigo.MaxLength = Reglas.LongitudCodigo;
            txtNombre.MaxLength = Reglas.LongitudNombre;
            txtCategoria.MaxLength = Reglas.LongitudCategoria;
            nudPrecio.Maximum = Reglas.PrecioMaximo;
            nudExistencias.Maximum = Reglas.MaximoExistencias;
        }

        public FrmProductos(ServicioProductos productos) : this()
        {
            this.productos = productos;
            ActualizarTabla();
            LimpiarEditor();
        }

        private void ActualizarTabla()
        {
            var lista = productos.Buscar(txtBuscar.Text);
            dgvProductos.DataSource = lista;
            dgvProductos.ClearSelection();
            lblCantidad.Text = lista.Count + " productos encontrados. Seleccione una fila para editar.";
        }

        private void LimpiarEditor()
        {
            productoEnEdicion = 0;
            txtCodigo.Clear();
            txtNombre.Clear();
            txtCategoria.Clear();
            nudPrecio.Value = 0m;
            nudExistencias.Value = 0m;
            errores.Clear();
            lblModo.Text = "Nuevo producto";
            btnGuardar.Text = "Registrar producto";
            btnEliminar.Enabled = false;
            dgvProductos.ClearSelection();
            txtCodigo.Focus();
        }

        private void DgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count == 0) return;
            Producto producto = dgvProductos.SelectedRows[0].DataBoundItem as Producto;
            if (producto == null) return;
            errores.Clear();
            productoEnEdicion = producto.Id;
            txtCodigo.Text = producto.Codigo;
            txtNombre.Text = producto.Nombre;
            txtCategoria.Text = producto.Categoria;
            nudPrecio.Value = producto.Precio;
            nudExistencias.Value = producto.Existencias;
            lblModo.Text = "Editar producto: " + producto.Codigo;
            btnGuardar.Text = "Guardar cambios";
            btnEliminar.Enabled = true;
        }

        private bool ValidarCampos()
        {
            errores.Clear();
            bool valido = true;
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            { errores.SetError(txtCodigo, "Ingrese el código."); valido = false; }
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            { errores.SetError(txtNombre, "Ingrese el nombre."); valido = false; }
            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            { errores.SetError(txtCategoria, "Ingrese la categoría."); valido = false; }
            if (nudPrecio.Value <= 0m)
            { errores.SetError(nudPrecio, "El precio debe ser mayor que cero."); valido = false; }
            if (!valido) MessageBox.Show(this, "Revise los campos señalados.", "Datos incompletos",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return valido;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;
            bool guardado = Mensajes.Ejecutar(this, () =>
            {
                // NumericUpDown restringe la entrada; Convert expresa la conversión a entero.
                int existencias = Convert.ToInt32(nudExistencias.Value);
                if (productoEnEdicion == 0)
                    productos.Registrar(txtCodigo.Text, txtNombre.Text, txtCategoria.Text, nudPrecio.Value, existencias);
                else
                    productos.Actualizar(productoEnEdicion, txtCodigo.Text, txtNombre.Text, txtCategoria.Text,
                        nudPrecio.Value, existencias);
            });
            if (guardado)
            {
                ActualizarTabla();
                LimpiarEditor();
                lblEstado.Text = "Producto guardado correctamente.";
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (productoEnEdicion == 0) return;
            if (!Mensajes.Confirmar(this, "¿Desea eliminar el producto " + txtCodigo.Text + "?\nLas ventas previas conservarán su detalle.")) return;
            if (Mensajes.Ejecutar(this, () => productos.Eliminar(productoEnEdicion)))
            {
                ActualizarTabla();
                LimpiarEditor();
                lblEstado.Text = "Producto eliminado.";
            }
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarEditor();
            lblEstado.Text = "Complete los campos para registrar otro producto.";
        }

        private void TxtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (productos == null) return;
            ActualizarTabla();
            LimpiarEditor();
            // Buscar no debe mover el foco al editor mientras se está escribiendo.
            txtBuscar.Focus();
        }

        private void BtnCerrar_Click(object sender, EventArgs e) { Close(); }
    }
}
