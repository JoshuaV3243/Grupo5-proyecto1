using SistemaVentasTienda.Core.Servicios;

namespace SistemaVentasTienda.Core.Datos
{
    public static class DatosDemostracion
    {
        /// <summary>Se llama una vez al iniciar. Facilita probar una venta sin registrar todo.</summary>
        public static void Cargar(ServicioProductos productos)
        {
            productos.Registrar("AB-001", "Arroz 1 kg", "Abarrotes", 950m, 30);
            productos.Registrar("AB-002", "Frijoles 900 g", "Abarrotes", 1350m, 20);
            productos.Registrar("BE-001", "Agua 600 ml", "Bebidas", 650m, 40);
            productos.Registrar("LI-001", "Jabón de manos", "Limpieza", 1200.50m, 15);
            productos.Registrar("PA-001", "Cuaderno", "Papelería", 1800m, 0);
        }
    }
}
