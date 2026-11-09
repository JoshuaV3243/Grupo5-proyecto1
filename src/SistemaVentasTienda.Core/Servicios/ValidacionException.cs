using System;

namespace SistemaVentasTienda.Core.Servicios
{
    /// <summary>Error esperado de los datos ingresados; el formulario muestra su mensaje.</summary>
    public class ValidacionException : Exception
    {
        public ValidacionException(string mensaje) : base(mensaje) { }
    }
}
