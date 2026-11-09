using System;
using System.Windows.Forms;
using SistemaVentasTienda.Core.Datos;
using SistemaVentasTienda.Core.Servicios;
using SistemaVentasTienda.WinForms.Formularios;

namespace SistemaVentasTienda.WinForms
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Una única instancia conecta inventario, ventas e historial durante la sesión.
            AlmacenMemoria almacen = new AlmacenMemoria();
            ServicioProductos productos = new ServicioProductos(almacen);
            ServicioVentas ventas = new ServicioVentas(almacen);
            DatosDemostracion.Cargar(productos);

            Application.Run(new FrmPrincipal(productos, ventas));
        }
    }
}
