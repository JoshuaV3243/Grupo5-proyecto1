using System;
using System.Collections.Generic;
using SistemaVentasTienda.Core.Datos;
using SistemaVentasTienda.Core.Modelos;

namespace SistemaVentasTienda.Core.Servicios
{
    public class ServicioProductos
    {
        private readonly AlmacenMemoria almacen;

        public ServicioProductos(AlmacenMemoria almacen)
        {
            this.almacen = almacen ?? throw new ArgumentNullException(nameof(almacen));
        }

        /// <summary>Busca por código, nombre o categoría; una búsqueda vacía muestra todo.</summary>
        public List<Producto> Buscar(string texto)
        {
            string filtro = (texto ?? "").Trim();
            List<Producto> resultado = new List<Producto>();
            foreach (Producto producto in almacen.Productos)
            {
                if (producto.Codigo.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                    || producto.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase)
                    || producto.Categoria.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                {
                    resultado.Add(producto.Copiar());
                }
            }
            return resultado;
        }

        public Producto Obtener(int id)
        {
            return EncontrarOriginal(id).Copiar();
        }

        /// <summary>Valida todos los campos antes de agregar un producto al inventario.</summary>
        public Producto Registrar(string codigo, string nombre, string categoria,
            decimal precio, int existencias)
        {
            Producto producto = Preparar(0, codigo, nombre, categoria, precio, existencias);
            producto.Id = almacen.SiguienteProductoId;
            almacen.Productos.Add(producto);
            almacen.SiguienteProductoId++;
            return producto.Copiar();
        }

        public void Actualizar(int id, string codigo, string nombre, string categoria,
            decimal precio, int existencias)
        {
            Producto original = EncontrarOriginal(id);
            Producto valido = Preparar(id, codigo, nombre, categoria, precio, existencias);
            // Se asigna después de validar: un error no deja el producto a medio editar.
            original.Codigo = valido.Codigo;
            original.Nombre = valido.Nombre;
            original.Categoria = valido.Categoria;
            original.Precio = valido.Precio;
            original.Existencias = valido.Existencias;
        }

        public void Eliminar(int id)
        {
            // Las ventas conservan una copia histórica; eliminar aquí no borra lo vendido.
            almacen.Productos.Remove(EncontrarOriginal(id));
        }

        private Producto Preparar(int id, string codigo, string nombre, string categoria,
            decimal precio, int existencias)
        {
            string codigoLimpio = Reglas.TextoObligatorio(codigo, "código", Reglas.LongitudCodigo).ToUpperInvariant();
            foreach (char caracter in codigoLimpio)
            {
                bool letra = caracter >= 'A' && caracter <= 'Z';
                bool digito = caracter >= '0' && caracter <= '9';
                if (!letra && !digito && caracter != '-' && caracter != '_')
                    throw new ValidacionException("El código admite letras A-Z, números, guion y guion bajo.");
            }

            foreach (Producto producto in almacen.Productos)
            {
                if (producto.Id != id && producto.Codigo.Equals(codigoLimpio, StringComparison.OrdinalIgnoreCase))
                    throw new ValidacionException("Ya existe un producto con ese código.");
            }

            string nombreLimpio = Reglas.TextoObligatorio(nombre, "nombre", Reglas.LongitudNombre);
            string categoriaLimpia = Reglas.TextoObligatorio(categoria, "categoría", Reglas.LongitudCategoria);
            Reglas.ValidarDinero(precio, "El precio", false);
            if (precio > Reglas.PrecioMaximo)
                throw new ValidacionException("El precio no puede superar " + Reglas.PrecioMaximo.ToString("N2") + ".");
            if (existencias < 0 || existencias > Reglas.MaximoExistencias)
                throw new ValidacionException("Las existencias deben estar entre 0 y " + Reglas.MaximoExistencias + ".");

            return new Producto { Id = id, Codigo = codigoLimpio, Nombre = nombreLimpio,
                Categoria = categoriaLimpia, Precio = precio, Existencias = existencias };
        }

        private Producto EncontrarOriginal(int id)
        {
            foreach (Producto producto in almacen.Productos)
            {
                if (producto.Id == id) return producto;
            }
            throw new ValidacionException("El producto seleccionado ya no existe.");
        }
    }
}
