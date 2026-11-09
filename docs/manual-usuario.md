# Manual de uso

## Inicio

Al abrir el sistema aparecen tres opciones: **Productos**, **Nueva venta** e
**Historial**. El menú resume los productos disponibles y las ventas del día.
Los datos de muestra sirven para probar el sistema, no representan una tienda real.

## Productos

1. Pulse **Nuevo / limpiar** para preparar un registro.
2. Ingrese código, nombre, categoría, precio y existencias. Los campos con `*`
   son obligatorios. El precio debe ser positivo; puede haber cero existencias.
3. Pulse **Registrar producto**.
4. Para editar, seleccione una fila. Corrija los datos y pulse **Guardar cambios**.
5. Para eliminar, seleccione una fila y pulse **Eliminar**. Confirme el mensaje.
   El historial de ventas conserva el artículo original.
6. Escriba en el buscador para filtrar por código, nombre o categoría; borre el
   texto para recuperar toda la lista.

Cambiar las existencias reemplaza el valor actual; no registra un movimiento de
entrada. Por ejemplo, si hay 8 unidades y agrega 2 físicamente, escriba 10.

Los iconos rojos señalan datos incompletos. Los errores de reglas, como un código
duplicado, aparecen en un mensaje. Los controles numéricos limitan los valores
y muestran los decimales de acuerdo con la configuración regional de Windows.

## Nueva venta

1. Busque un producto y seleccione su fila del inventario.
2. Indique la cantidad y pulse **Agregar al carrito**.
3. Repetir un producto suma cantidades. El inventario todavía no se descuenta.
4. Para cambiar una cantidad, seleccione su fila del carrito, escriba el nuevo
   valor y pulse **Cambiar cantidad**. También puede **Quitar producto** o
   **Vaciar carrito**.
5. Escriba el cliente. Puede conservar **Consumidor final**. Seleccione la fecha
   y el medio de pago.
6. En efectivo, ingrese el dinero recibido. El sistema muestra el cambio o lo
   que falta. En tarjeta y transferencia el importe es igual al total.
7. Pulse **Confirmar venta** y acepte. Una venta válida guarda su número,
   descuenta las existencias y limpia el carrito.

Las validaciones fallidas conservan el carrito. Revise los datos y vuelva a
confirmar. Si intenta salir con artículos pendientes, debe confirmar que desea
descartarlos. Volver al menú sin confirmar una venta no descuenta inventario.

## Ejemplo con los datos iniciales

Este recorrido parte de una ejecución nueva, sin cambios previos:

| Acción | Resultado esperado |
| --- | --- |
| Agregar 2 unidades de `AB-001` (Arroz 1 kg) | Subtotal CRC 1 900,00 |
| Agregar 1 unidad de `BE-001` (Agua 600 ml) | Subtotal CRC 650,00 |
| Elegir efectivo e ingresar CRC 3 000,00 | Total CRC 2 550,00; cambio CRC 450,00 |
| Confirmar | Venta `V-00001`; arroz: 28 unidades; agua: 39 |
| Consultar Historial | Una venta con dos filas de detalle |

El separador decimal y los separadores de miles dependen de Windows. Los valores
numéricos del ejemplo se mantienen aunque cambie su representación.

## Historial

Al abrir se muestran las ventas de hoy. Elija **Desde**, **Hasta** y, si lo
necesita, un fragmento del número o cliente; pulse **Buscar**. **Todas las fechas**
quita esos filtros y consulta la sesión completa. Seleccione una venta para
ver sus productos, importes, monto recibido y cambio. El resumen inferior suma
únicamente las ventas filtradas.

## Cierre

**Salir del sistema** o la `X` del menú muestran una confirmación. Al cerrar se
pierden los datos de la sesión. La siguiente apertura vuelve a cargar únicamente
el inventario de demostración. No existe un guardado en disco en este avance.
