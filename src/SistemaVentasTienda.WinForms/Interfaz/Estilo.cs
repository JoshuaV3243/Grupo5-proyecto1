using System.Drawing;
using System.Windows.Forms;

namespace SistemaVentasTienda.WinForms.Interfaz
{
    /// <summary>Apariencia compartida para no repetir colores y formato en cada formulario.</summary>
    internal static class Estilo
    {
        public static readonly Color Azul = Color.FromArgb(29, 53, 87);
        public static readonly Color Verde = Color.FromArgb(0, 112, 99);
        public static readonly Color Fondo = Color.FromArgb(245, 247, 250);
        public static readonly Color Rojo = Color.FromArgb(174, 43, 51);

        public static void Aplicar(Form formulario)
        {
            formulario.BackColor = Fondo;
            formulario.Font = new Font("Segoe UI", 9F);
        }

        public static void BotonPrincipal(Button boton)
        {
            boton.BackColor = Verde;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
        }

        public static void BotonPeligro(Button boton)
        {
            boton.ForeColor = Rojo;
        }

        public static void PrepararTabla(DataGridView tabla)
        {
            tabla.AutoGenerateColumns = false;
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.AllowUserToResizeRows = false;
            tabla.MultiSelect = false;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.RowHeadersVisible = false;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabla.BackgroundColor = Color.White;
            tabla.BorderStyle = BorderStyle.FixedSingle;
            tabla.EnableHeadersVisualStyles = false;
            tabla.ColumnHeadersDefaultCellStyle.BackColor = Azul;
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tabla.ColumnHeadersDefaultCellStyle.Padding = new Padding(4);
            tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tabla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 237, 233);
            tabla.DefaultCellStyle.SelectionForeColor = Color.Black;
            tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 249, 251);
            tabla.RowTemplate.Height = 28;
        }

        public static void Columna(DataGridView tabla, string propiedad, string titulo,
            float peso, string formato = "")
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn();
            columna.Name = propiedad;
            columna.DataPropertyName = propiedad;
            columna.HeaderText = titulo;
            columna.FillWeight = peso;
            columna.MinimumWidth = 55;
            columna.SortMode = DataGridViewColumnSortMode.NotSortable;
            if (formato != "")
            {
                columna.DefaultCellStyle.Format = formato;
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            tabla.Columns.Add(columna);
        }
    }
}
