using System;
using System.Diagnostics;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Servicios;

namespace SistemaVentasTienda.WinForms.Interfaz
{
    internal static class Mensajes
    {
        /// <summary>Captura errores en el límite entre el formulario y la lógica.</summary>
        public static bool Ejecutar(IWin32Window ventana, Action accion)
        {
            try
            {
                accion();
                return true;
            }
            catch (ValidacionException ex)
            {
                MessageBox.Show(ventana, ex.Message, "Revise los datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessageBox.Show(ventana, "No se pudo completar la operación.\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public static bool Confirmar(IWin32Window ventana, string texto)
        {
            return MessageBox.Show(ventana, texto, "Confirmación", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }
}
