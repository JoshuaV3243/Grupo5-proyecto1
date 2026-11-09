# Verificación de la primera versión

## Comprobaciones ejecutadas

Se compiló la solución completa con el SDK .NET 8.0.427 desde Linux utilizando
el paquete de referencias de Windows Forms. Resultado: **0 errores y 0 advertencias**.
Se ejecutaron las **38 pruebas de lógica**, todas aprobadas.

```powershell
dotnet build SistemaVentasTienda.sln --configuration Release -m:1 -p:TreatWarningsAsErrors=true
dotnet run --project tests/SistemaVentasTienda.Pruebas --configuration Release --no-build
```

Los casos revisan: entradas obligatorias; códigos duplicados; límites y longitudes;
ediciones rechazadas sin cambios parciales; búsquedas y eliminación; cantidades y
existencias acumuladas; exactitud de centavos; pagos; fechas; confirmación y
numeración; revalidación de inventario y precio; copias históricas; filtros; y el
importe máximo de un carrito. El código fuente de las pruebas permite repetirlas.

Se utilizó compilación secuencial (`-m:1`) para evitar un fallo de restauración
observado al compilar en paralelo en este entorno. No modifica la lógica del
sistema. El workflow utiliza la misma opción para una ejecución uniforme.

## Revisión manual pendiente en Windows

Compilar desde Linux no ejecuta Windows Forms. No se ha verificado la apariencia
en Windows, la apertura en el diseñador de Visual Studio ni el uso real de los
controles. Tampoco se ha ejecutado el workflow en GitHub. La pareja debe completar
este recorrido y anotar el resultado real antes de presentar el avance.

| Paso | Qué comprobar | Estado |
| --- | --- | --- |
| Abrir la solución y establecer WinForms como inicio | Compila y abre el menú con F5 | Pendiente |
| Abrir los cuatro formularios en el diseñador | Controles y eventos editables | Pendiente |
| Navegar y redimensionar las ventanas | Textos legibles y controles accesibles con la escala de Windows usada | Pendiente |
| Registrar un producto nuevo | Se muestra en la tabla y puede buscarse | Pendiente |
| Repetir el código, dejar campos vacíos e ingresar precio cero | Se rechaza y se conserva el inventario | Pendiente |
| Editar precio y existencias; eliminar con confirmación | La tabla refleja los cambios | Pendiente |
| Ejecutar el ejemplo del manual | Total 2 550,00; cambio 450,00; existencias 28 y 39 | Pendiente |
| Intentar vender más existencias o pagar menos | Mensaje claro; venta sin registrar | Pendiente |
| Cambiar cantidades, quitar artículos y vaciar | Totales actualizados e inventario sin cambios antes de confirmar | Pendiente |
| Registrar tarjeta y transferencia | Importe igual al total y cambio cero | Pendiente |
| Filtrar historial y seleccionar una venta | Fechas, cliente, total y detalles correctos | Pendiente |
| Invertir el período de búsqueda | Se rechaza el período | Pendiente |
| Cerrar una venta pendiente | Puede cancelar el cierre o descartar el carrito | Pendiente |
| Salir y reabrir | Advierte que los datos se pierden y restaura la demostración | Pendiente |
| Publicar y abrir un PR en GitHub | Workflow aprobado y revisión del compañero | Pendiente |

## Resultados del equipo

Completen después de probar, sin marcar como realizado un paso pendiente:

- Integrante que ejecutó la revisión: ____________________
- Fecha y versión de Windows / Visual Studio: ____________________
- Resultado y observaciones: ____________________
- Correcciones y enlace al PR: ____________________
