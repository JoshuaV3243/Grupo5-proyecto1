# Correspondencia con el PDF

Se revisaron las cuatro páginas de `promptproject1.pdf`. El archivo especifica
requisitos técnicos, trabajo con Git/GitHub y la primera entrega funcional.

## Requisitos técnicos y programación (páginas 1 y 2)

| Requisito | Implementación |
| --- | --- |
| C#, Windows Forms, .NET y Visual Studio | Solución `.sln`, proyectos Core y WinForms |
| Variables y tipos de datos | `int`, `long`, `string`, `decimal`, `bool`, `DateTime`, enum y listas |
| Operadores y conversiones | Cálculos de total y cambio; `Convert.ToInt32` en eventos de cantidades |
| Métodos, parámetros y retornos | `Registrar`, `Buscar`, `CalcularTotal` y `ConfirmarVenta` |
| `if`, `else` y `switch` | Servicios, guardado de productos y descripción de medios de pago |
| `for`, `foreach` o `while` | `foreach` para inventario y total; `for` para recorrer ventas |
| Fechas y cálculos | Fecha de venta, período de historial, subtotales, total y cambio |
| Búsquedas, modificaciones y eliminaciones | Mantenimiento de productos y del carrito |
| Datos obligatorios, numéricos, fechas, valores, longitud y duplicados | `Reglas`, `ServicioProductos`, `ServicioVentas` y controles restringidos |
| ErrorProvider, MessageBox y try-catch | Validaciones visuales en Productos; helper `Mensajes` |
| Separación de presentación, lógica y datos | WinForms, Core/Servicios y Core/Datos |

## Aplicación del Avance 1 (páginas 3 y 4)

| Mínimo solicitado | Dónde se cumple |
| --- | --- |
| Formulario principal y navegación | `FrmPrincipal`: productos, nueva venta e historial |
| Formularios necesarios | `FrmProductos`, `FrmVentas`, `FrmHistorial` |
| Controles y eventos | Botones, cajas de texto, numéricos, fechas, combos y eventos asociados |
| MessageBox | Errores, confirmaciones de venta, eliminación y salida |
| Limpieza de controles | Nuevo producto, carrito vacío y preparación de la siguiente venta |
| Salida del sistema | Botón de salida y evento `FormClosing` |
| Una funcionalidad real | Registrar venta, descontar existencias y consultar su detalle |
| Listas o colecciones en memoria | Productos y ventas en `AlmacenMemoria`; carrito en `ServicioVentas` |
| Presentación organizada | Tablas DataGridView con columnas explícitas y formatos numéricos |
| Sistema integrado | Todos los módulos reciben los mismos servicios y almacén |

## Documentación, Git y GitHub (páginas 2 y 3)

| Solicitud | Estado |
| --- | --- |
| README: nombre, descripción, objetivo, integrantes, tecnologías, funcionalidades, estado, estructura y ejecución | Incluido; faltan los nombres reales de ambos |
| `docs/propuesta.md`: nombre, integrantes, problema, justificación, usuarios, objetivo general y específicos | Incluido; faltan los nombres reales |
| Propuesta: alcance, fuera de alcance, funcionalidades y distribución inicial de tareas | Incluido |
| Documentación Markdown actualizada | Propuesta, arquitectura, manual, colaboración, requisitos y verificación |
| Git desde el inicio y commits significativos | Repositorio local iniciado antes de la lógica; historial por etapas de preparación |
| GitHub y participación de todos | Pendiente de publicación y aportes reales de los dos integrantes |
| Evolución, cambios y correcciones | La base conserva sus etapas y correcciones; el equipo debe continuar registrando su desarrollo |

## Criterios de evaluación

La propuesta cubre alcance; las pantallas cubren navegación y eventos; Core
contiene las estructuras de C#, métodos, validaciones y listas; los proyectos
separan responsabilidades; los documentos y el historial preparan la colaboración.
La evaluación del trabajo real en GitHub depende de la actividad posterior de
la pareja y de los criterios del docente. No se da por completado ese requisito.
