namespace SistemaVentasTienda.Core.Modelos
{
    public enum MedioPago
    {
        Efectivo,
        Tarjeta,
        Transferencia
    }

    public static class DescripcionPago
    {
        public static string Obtener(MedioPago medio)
        {
            // switch convierte el valor interno en el texto mostrado en las tablas.
            switch (medio)
            {
                case MedioPago.Efectivo: return "Efectivo";
                case MedioPago.Tarjeta: return "Tarjeta";
                case MedioPago.Transferencia: return "Transferencia";
                default: return "Desconocido";
            }
        }
    }
}
