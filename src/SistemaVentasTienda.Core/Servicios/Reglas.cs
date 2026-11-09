using System;

namespace SistemaVentasTienda.Core.Servicios
{
    /// <summary>Límites compartidos por las reglas y los controles de Windows Forms.</summary>
    public static class Reglas
    {
        public const int LongitudCodigo = 20;
        public const int LongitudNombre = 100;
        public const int LongitudCategoria = 50;
        public const int LongitudCliente = 100;
        public const int MaximoExistencias = 100000;
        public const int MaximoLineas = 100;
        public const decimal PrecioMaximo = 1000000m;
        public const decimal MontoMaximo = 10000000000000m;
        public static readonly DateTime FechaMinima = new DateTime(2000, 1, 1);

        public static string TextoObligatorio(string texto, string campo, int longitud)
        {
            string limpio = (texto ?? "").Trim();
            if (limpio.Length == 0)
                throw new ValidacionException("El campo " + campo + " es obligatorio.");
            if (limpio.Length > longitud)
                throw new ValidacionException(campo + " permite hasta " + longitud + " caracteres.");
            foreach (char caracter in limpio)
            {
                if (char.IsControl(caracter))
                    throw new ValidacionException(campo + " no admite saltos de línea ni caracteres de control.");
            }
            return limpio;
        }

        public static void ValidarDinero(decimal valor, string campo, bool permiteCero)
        {
            if (valor < 0m || (!permiteCero && valor == 0m) || valor > MontoMaximo)
                throw new ValidacionException(campo + " tiene un valor fuera del rango permitido.");
            if (decimal.Round(valor, 2) != valor)
                throw new ValidacionException(campo + " admite como máximo dos decimales.");
        }

        public static void ValidarPeriodo(DateTime desde, DateTime hasta)
        {
            if (desde.Date < FechaMinima || hasta.Date > DateTime.Today)
                throw new ValidacionException("El período debe estar entre el 01/01/2000 y hoy.");
            if (desde.Date > hasta.Date)
                throw new ValidacionException("La fecha inicial no puede ser posterior a la fecha final.");
        }
    }
}
