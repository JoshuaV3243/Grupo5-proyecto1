using System;
using SistemaVentasTienda.Core.Datos;
using SistemaVentasTienda.Core.Modelos;
using SistemaVentasTienda.Core.Servicios;

namespace SistemaVentasTienda.Pruebas
{
    /// <summary>
    /// Pruebas de reglas sin paquetes externos. Cada caso usa datos nuevos.
    /// Devuelve un código distinto de cero si falla, también en GitHub Actions.
    /// </summary>
    internal static class Program
    {
        private static int aprobadas;
        private static int fallidas;

        private static int Main()
        {
            Probar("Normaliza código y espacios", () =>
            {
                Escenario e = new Escenario();
                Producto p = e.Productos.Registrar(" ab-01 ", " Arroz ", " Alimentos ", 0.10m, 5);
                Igual("AB-01", p.Codigo); Igual("Arroz", p.Nombre); Igual("Alimentos", p.Categoria);
            });
            Probar("Impide códigos duplicados sin distinguir mayúsculas", () =>
            {
                Escenario e = new Escenario(); e.Agregar();
                Rechaza(() => e.Productos.Registrar(" p-01 ", "Otro", "Prueba", 1m, 1));
                Igual(1, e.Productos.Buscar("").Count);
            });
            Probar("Rechaza símbolos y espacios internos en códigos", () =>
            {
                Escenario e = new Escenario();
                foreach (string codigo in new[] { "A B", "Á-1", "A/1", "A.1" })
                    Rechaza(() => e.Productos.Registrar(codigo, "Artículo", "Prueba", 1m, 1));
            });
            Probar("Valida campos obligatorios y caracteres de control", () =>
            {
                Escenario e = new Escenario();
                Rechaza(() => e.Productos.Registrar("", "A", "B", 1m, 1));
                Rechaza(() => e.Productos.Registrar("A", " ", "B", 1m, 1));
                Rechaza(() => e.Productos.Registrar("A", "A", null, 1m, 1));
                Rechaza(() => e.Productos.Registrar("A", "Uno\nDos", "B", 1m, 1));
            });
            Probar("Valida longitudes de código, nombre y categoría", () =>
            {
                Escenario e = new Escenario();
                Rechaza(() => e.Productos.Registrar(new string('A', 21), "A", "B", 1m, 1));
                Rechaza(() => e.Productos.Registrar("A", new string('A', 101), "B", 1m, 1));
                Rechaza(() => e.Productos.Registrar("A", "A", new string('B', 51), 1m, 1));
            });
            Probar("Valida precios positivos, límite y decimales", () =>
            {
                Escenario e = new Escenario();
                foreach (decimal precio in new[] { 0m, -1m, 1000000.01m, 1.001m })
                    Rechaza(() => e.Productos.Registrar("A", "A", "B", precio, 1));
                e.Productos.Registrar("A", "A", "B", 1000000m, 0);
            });
            Probar("Valida límites de existencias", () =>
            {
                Escenario e = new Escenario();
                Rechaza(() => e.Productos.Registrar("A", "A", "B", 1m, -1));
                Rechaza(() => e.Productos.Registrar("A", "A", "B", 1m, 100001));
                e.Productos.Registrar("A", "A", "B", 1m, 100000);
            });
            Probar("Busca por código, nombre y categoría", () =>
            {
                Escenario e = new Escenario(); e.Productos.Registrar("AR-1", "Arroz", "Alimentos", 1m, 1);
                Igual(1, e.Productos.Buscar("ar-").Count); Igual(1, e.Productos.Buscar("ROZ").Count);
                Igual(1, e.Productos.Buscar("aliMENtos").Count); Igual(0, e.Productos.Buscar("xyz").Count);
            });
            Probar("Una edición admite su propio código", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar();
                e.Productos.Actualizar(p.Id, "p-01", "Actualizado", "Nueva", 20.50m, 7);
                Igual("Actualizado", e.Productos.Obtener(p.Id).Nombre); Igual(7, e.Stock(p.Id));
            });
            Probar("Una edición rechazada no cambia otros campos", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Agregar("P-02");
                Rechaza(() => e.Productos.Actualizar(p.Id, "P-02", "Incorrecto", "X", 3m, 0));
                Igual("Producto P-01", e.Productos.Obtener(p.Id).Nombre); Igual(10, e.Stock(p.Id));
            });
            Probar("Eliminar no reutiliza identificadores", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Productos.Eliminar(p.Id);
                Rechaza(() => e.Productos.Obtener(p.Id));
                Producto siguiente = e.Agregar("P-02"); Igual(p.Id + 1, siguiente.Id);
            });
            Probar("Vaciar resultados no borra el inventario", () =>
            {
                Escenario e = new Escenario(); e.Agregar(); e.Productos.Buscar("").Clear();
                Igual(1, e.Productos.Buscar("").Count);
            });
            Probar("Agregar repetidos suma cantidades sin descontar stock", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar();
                e.Ventas.AgregarProducto(p.Id, 2); e.Ventas.AgregarProducto(p.Id, 3);
                Igual(1, e.Ventas.ObtenerCarrito().Count); Igual(5, e.Ventas.ObtenerCarrito()[0].Cantidad);
                Igual(10, e.Stock(p.Id)); Igual(50m, e.Ventas.CalcularTotal());
            });
            Probar("Rechaza sobreventa acumulada y conserva el carrito", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 6);
                Rechaza(() => e.Ventas.AgregarProducto(p.Id, 5));
                Igual(6, e.Ventas.ObtenerCarrito()[0].Cantidad); Igual(10, e.Stock(p.Id));
            });
            Probar("Rechaza productos agotados o inexistentes", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(stock: 0);
                Rechaza(() => e.Ventas.AgregarProducto(p.Id, 1));
                Rechaza(() => e.Ventas.AgregarProducto(999, 1));
            });
            Probar("Las cantidades deben ser positivas y acotadas", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar();
                foreach (int cantidad in new[] { 0, -1, 100001, int.MaxValue })
                    Rechaza(() => e.Ventas.AgregarProducto(p.Id, cantidad));
            });
            Probar("Cambiar cantidad recalcula el total", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 2);
                e.Ventas.CambiarCantidad(p.Id, 4); Igual(40m, e.Ventas.CalcularTotal());
                Rechaza(() => e.Ventas.CambiarCantidad(p.Id, 11));
                Rechaza(() => e.Ventas.CambiarCantidad(p.Id, 0)); Igual(40m, e.Ventas.CalcularTotal());
            });
            Probar("Quitar y vaciar carrito conservan las existencias", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 2);
                e.Ventas.QuitarProducto(p.Id); Igual(0m, e.Ventas.CalcularTotal());
                e.Ventas.AgregarProducto(p.Id, 1); e.Ventas.LimpiarCarrito(); Igual(10, e.Stock(p.Id));
                Igual(0, e.Ventas.ObtenerCarrito().Count);
            });
            Probar("Vaciar la copia del carrito no modifica el original", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                e.Ventas.ObtenerCarrito().Clear(); Igual(10m, e.Ventas.CalcularTotal());
            });
            Probar("Impide confirmar un carrito vacío", () =>
            {
                Escenario e = new Escenario(); Rechaza(() => e.Confirmar(10m)); Igual(0, e.Historial());
            });
            Probar("Confirma efectivo, calcula cambio y descuenta una sola vez", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 3);
                Venta venta = e.Confirmar(50m); Igual(30m, venta.Total); Igual(20m, venta.Cambio);
                Igual(7, e.Stock(p.Id)); Igual(0, e.Ventas.ObtenerCarrito().Count); Igual(1, e.Historial());
                Rechaza(() => e.Confirmar(50m)); Igual(7, e.Stock(p.Id));
            });
            Probar("Calcula centavos exactamente con decimal", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(precio: 0.10m);
                e.Ventas.AgregarProducto(p.Id, 3); Venta venta = e.Confirmar(0.50m);
                Igual(0.30m, venta.Total); Igual(0.20m, venta.Cambio);
            });
            Probar("Un pago insuficiente conserva inventario, carrito e historial", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 2);
                Rechaza(() => e.Confirmar(19m)); Igual(10, e.Stock(p.Id)); Igual(0, e.Historial());
                Igual(1, e.Ventas.ObtenerCarrito().Count);
            });
            Probar("Valida montos negativos, cero, exceso y fracciones de centavo", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                foreach (decimal monto in new[] { -1m, 0m, 10.001m, Reglas.MontoMaximo + 1m })
                    Rechaza(() => e.Confirmar(monto));
            });
            Probar("Tarjeta y transferencia exigen exactamente el total", () =>
            {
                foreach (MedioPago medio in new[] { MedioPago.Tarjeta, MedioPago.Transferencia })
                {
                    Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                    Rechaza(() => e.Ventas.ConfirmarVenta("Cliente", DateTime.Today, medio, 11m));
                    Venta venta = e.Ventas.ConfirmarVenta("Cliente", DateTime.Today, medio, 10m);
                    Igual(0m, venta.Cambio); Igual(medio, venta.MedioPago);
                }
            });
            Probar("Rechaza un medio de pago desconocido", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                Rechaza(() => e.Ventas.ConfirmarVenta("Cliente", DateTime.Today, (MedioPago)99, 10m));
            });
            Probar("Valida cliente vacío y longitud", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                Rechaza(() => e.Ventas.ConfirmarVenta(" ", DateTime.Today, MedioPago.Efectivo, 10m));
                Rechaza(() => e.Ventas.ConfirmarVenta(new string('A', 101), DateTime.Today, MedioPago.Efectivo, 10m));
            });
            Probar("Valida fechas futuras y anteriores al mínimo", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                Rechaza(() => e.Ventas.ConfirmarVenta("Cliente", DateTime.Today.AddDays(1), MedioPago.Efectivo, 10m));
                Rechaza(() => e.Ventas.ConfirmarVenta("Cliente", Reglas.FechaMinima.AddDays(-1), MedioPago.Efectivo, 10m));
            });
            Probar("Revalida todas las existencias antes de descontar cualquiera", () =>
            {
                Escenario e = new Escenario(); Producto a = e.Agregar(); Producto b = e.Agregar("P-02");
                e.Ventas.AgregarProducto(a.Id, 2); e.Ventas.AgregarProducto(b.Id, 3);
                e.Productos.Actualizar(b.Id, b.Codigo, b.Nombre, b.Categoria, b.Precio, 1);
                Rechaza(() => e.Confirmar(100m)); Igual(10, e.Stock(a.Id)); Igual(1, e.Stock(b.Id));
                Igual(0, e.Historial()); Igual(2, e.Ventas.ObtenerCarrito().Count);
            });
            Probar("Rechaza un producto eliminado sin venta parcial", () =>
            {
                Escenario e = new Escenario(); Producto a = e.Agregar(); Producto b = e.Agregar("P-02");
                e.Ventas.AgregarProducto(a.Id, 1); e.Ventas.AgregarProducto(b.Id, 1); e.Productos.Eliminar(b.Id);
                Rechaza(() => e.Confirmar(100m)); Igual(10, e.Stock(a.Id)); Igual(0, e.Historial());
            });
            Probar("Un precio modificado exige actualizar el carrito", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                e.Productos.Actualizar(p.Id, p.Codigo, p.Nombre, p.Categoria, 25m, p.Existencias);
                Rechaza(() => e.Confirmar(100m)); Igual(10, e.Stock(p.Id)); Igual(0, e.Historial());
                e.Ventas.QuitarProducto(p.Id); e.Ventas.AgregarProducto(p.Id, 1); Igual(25m, e.Confirmar(25m).Total);
            });
            Probar("El historial conserva precios y nombres tras editar y eliminar", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 2);
                Venta venta = e.Confirmar(20m);
                e.Productos.Actualizar(p.Id, "NUEVO", "Otro nombre", "Otra", 99m, 5); e.Productos.Eliminar(p.Id);
                Igual("P-01", venta.Detalles[0].Codigo); Igual("Producto P-01", venta.Detalles[0].Nombre);
                Igual(10m, venta.Detalles[0].PrecioUnitario); Igual(20m, venta.Total); Igual(1, e.Historial());
            });
            Probar("Los identificadores de venta avanzan solo al confirmar", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); e.Ventas.AgregarProducto(p.Id, 1);
                Rechaza(() => e.Confirmar(1m)); Igual("V-00001", e.Confirmar(10m).Numero);
                e.Ventas.AgregarProducto(p.Id, 1); Igual("V-00002", e.Confirmar(10m).Numero);
            });
            Probar("Filtra fechas inclusivas, número y cliente", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); DateTime ayer = DateTime.Today.AddDays(-1);
                e.Ventas.AgregarProducto(p.Id, 1); e.Ventas.ConfirmarVenta("Ana Pérez", ayer, MedioPago.Efectivo, 10m);
                e.Ventas.AgregarProducto(p.Id, 1); e.Confirmar(10m);
                Igual(1, e.Ventas.BuscarVentas(ayer, ayer, "ana").Count);
                Igual(2, e.Ventas.BuscarVentas(ayer, DateTime.Today, "V-0000").Count);
                Igual(0, e.Ventas.BuscarVentas(DateTime.Today, DateTime.Today, "Ana").Count);
                Igual("V-00002", e.Ventas.BuscarVentas(ayer, DateTime.Today, "")[0].Numero);
            });
            Probar("Rechaza períodos invertidos y fuera de rango", () =>
            {
                Escenario e = new Escenario();
                Rechaza(() => e.Ventas.BuscarVentas(DateTime.Today, DateTime.Today.AddDays(-1), ""));
                Rechaza(() => e.Ventas.BuscarVentas(Reglas.FechaMinima.AddDays(-1), DateTime.Today, ""));
                Rechaza(() => e.Ventas.BuscarVentas(DateTime.Today, DateTime.Today.AddDays(1), ""));
            });
            Probar("Comparte inventario entre servicios de la misma sesión", () =>
            {
                Escenario e = new Escenario(); Producto p = e.Agregar(); ServicioVentas otraCaja = new ServicioVentas(e.Almacen);
                e.Ventas.AgregarProducto(p.Id, 7); otraCaja.AgregarProducto(p.Id, 7);
                otraCaja.ConfirmarVenta("Cliente", DateTime.Today, MedioPago.Efectivo, 70m);
                Rechaza(() => e.Confirmar(70m)); Igual(3, e.Stock(p.Id)); Igual(1, e.Historial());
            });
            Probar("Limita productos distintos y acepta el total monetario máximo", () =>
            {
                Escenario e = new Escenario();
                for (int i = 0; i < Reglas.MaximoLineas; i++)
                {
                    Producto p = e.Agregar("P-" + i, Reglas.PrecioMaximo, Reglas.MaximoExistencias);
                    e.Ventas.AgregarProducto(p.Id, Reglas.MaximoExistencias);
                }
                Producto extra = e.Agregar("EXTRA"); Rechaza(() => e.Ventas.AgregarProducto(extra.Id, 1));
                Igual(Reglas.MontoMaximo, e.Confirmar(Reglas.MontoMaximo).Total);
            });
            Probar("El inventario de demostración permite probar una venta", () =>
            {
                Escenario e = new Escenario(); DatosDemostracion.Cargar(e.Productos);
                Igual(5, e.Productos.Buscar("").Count); Producto arroz = e.Productos.Buscar("AB-001")[0];
                e.Ventas.AgregarProducto(arroz.Id, 2); Venta venta = e.Confirmar(2000m);
                Igual(1900m, venta.Total); Igual(100m, venta.Cambio); Igual(28, e.Stock(arroz.Id));
            });

            Console.WriteLine("\nResultado: " + aprobadas + " aprobadas; " + fallidas + " fallidas.");
            return fallidas == 0 ? 0 : 1;
        }

        private static void Probar(string nombre, Action prueba)
        {
            try { prueba(); aprobadas++; Console.WriteLine("OK: " + nombre); }
            catch (Exception ex) { fallidas++; Console.WriteLine("ERROR: " + nombre + " -> " + ex.Message); }
        }

        private static void Igual<T>(T esperado, T obtenido)
        {
            if (!object.Equals(esperado, obtenido))
                throw new Exception("Se esperaba " + esperado + " y se obtuvo " + obtenido + ".");
        }

        private static void Rechaza(Action accion)
        {
            try { accion(); }
            catch (ValidacionException) { return; }
            throw new Exception("La operación debía rechazar los datos.");
        }

        private sealed class Escenario
        {
            public AlmacenMemoria Almacen { get; } = new AlmacenMemoria();
            public ServicioProductos Productos { get; }
            public ServicioVentas Ventas { get; }

            public Escenario()
            {
                Productos = new ServicioProductos(Almacen);
                Ventas = new ServicioVentas(Almacen);
            }

            public Producto Agregar(string codigo = "P-01", decimal precio = 10m, int stock = 10)
            {
                return Productos.Registrar(codigo, "Producto " + codigo, "Prueba", precio, stock);
            }

            public int Stock(int id) { return Productos.Obtener(id).Existencias; }
            public int Historial() { return Ventas.BuscarVentas(Reglas.FechaMinima, DateTime.Today, "").Count; }
            public Venta Confirmar(decimal monto)
            {
                return Ventas.ConfirmarVenta("Consumidor final", DateTime.Today, MedioPago.Efectivo, monto);
            }
        }
    }
}
