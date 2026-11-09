# Organización del código

La solución separa responsabilidades para que los integrantes puedan comprender
y modificar un módulo sin mezclar los cálculos con los eventos de la pantalla.

| Proyecto o carpeta | Responsabilidad |
| --- | --- |
| `Core/Modelos` | Producto, línea de carrito, venta, detalle y medio de pago |
| `Core/Datos` | Listas compartidas e inventario de demostración |
| `Core/Servicios` | Validaciones, búsquedas, mantenimiento, cálculos y confirmación |
| `WinForms` | Formularios, presentación de tablas y eventos |
| `Pruebas` | Recorridos verificables de inventario y ventas, sin interfaz gráfica |

## Reglas de la primera versión

- El código del producto es único, se normaliza a mayúsculas y admite A-Z, 0-9,
  guion y guion bajo (máximo 20 caracteres).
- Nombre: 100 caracteres; categoría: 50; cliente: 100. Son obligatorios.
  Para compradores sin identificación se utiliza el texto `Consumidor final`.
- Precio: mayor que cero, hasta 1 000 000; existencias: 0 a 100 000.
- El dinero utiliza `decimal` con un máximo de dos decimales.
- El carrito admite hasta 100 productos distintos. Agregar el mismo suma unidades.
- Agregar, quitar o vaciar el carrito no descuenta inventario.
- Confirmar revalida TODOS los productos y después descuenta existencias.
- Si cambia un producto en el inventario, debe quitarse y volver a agregarse al
  carrito. No se actualiza un precio silenciosamente.
- La fecha de venta está entre 01/01/2000 y hoy. El historial valida el período.
- Efectivo exige un monto suficiente y calcula el cambio. Tarjeta y transferencia
  registran exactamente el total: solo se anota el medio, sin procesar un pago real.
- Las ventas confirmadas no se modifican ni eliminan. Cada detalle guarda el nombre,
  código y precio originales, aunque después se cambie o elimine el producto.
- Los importes son precios finales de demostración en colones (CRC). No se calcula
  IVA, descuentos ni facturación fiscal en este avance.

## Flujo de datos

`Program` crea un único `AlmacenMemoria` y los servicios. El menú principal entrega
esos mismos servicios a los formularios. Así, una venta se refleja inmediatamente
en productos e historial. Los módulos se abren como ventanas modales: el usuario
termina o cancela una operación antes de abrir otra.

Los servicios entregan copias de productos y del carrito. Los detalles históricos
son de solo lectura. Esto evita cambiar inventario o ventas desde una tabla.

## Límites deliberados

Los datos solo viven durante la ejecución y se pierden al cerrar. Esta versión
no es un sistema para producción ni almacena información entre computadoras.
La atomicidad de las validaciones corresponde a una aplicación de una sola
sesión y un solo hilo de interfaz; no es una transacción de base de datos.
