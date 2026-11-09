# Sistema de ventas en tienda

Aplicación de escritorio en C# y Windows Forms para administrar productos y
registrar ventas de una tienda pequeña. Corresponde al **Avance 1: primera
versión funcional** del documento de requisitos.

## Objetivo

Controlar productos y existencias, calcular una venta y consultar su detalle
desde una misma aplicación, con validaciones y datos en memoria.

## Integrantes

- Integrante 1: **pendiente de completar por el equipo**.
- Integrante 2: **pendiente de completar por el equipo**.

## Tecnologías

- C# y .NET 8.
- Windows Forms y Visual Studio Community 2022, versión 17.8 o posterior.
- Git y GitHub para control de versiones y colaboración.
- Markdown para documentación.
- Listas en memoria; sin base de datos y sin paquetes NuGet de terceros.

## Funcionalidades iniciales

- Menú principal con navegación, resumen de inventario y ventas del día.
- Registro, búsqueda, modificación y eliminación de productos.
- Validaciones de campos, longitudes, códigos duplicados, precio y existencias.
- Carrito con cantidades, cambio de cantidad, eliminación y limpieza.
- Venta en efectivo con cálculo de cambio; registro de tarjeta y transferencia.
- Descuento de existencias al confirmar la venta.
- Historial con filtros por fecha, número y cliente; detalle histórico de artículos.
- Tablas `DataGridView`, `ErrorProvider`, mensajes y confirmaciones de salida.

## Estado

Implementación del Avance 1 terminada. Compilación completa verificada con cero
errores y cero advertencias; 38 pruebas de lógica aprobadas. La revisión visual
y el recorrido manual en Windows están pendientes: el entorno de preparación
utiliza Linux. El workflow de GitHub está preparado y aún no se ha ejecutado en
un repositorio remoto.

**Los datos se pierden al cerrar la aplicación.** Cada ejecución comienza con
cinco productos de demostración y sin ventas. Todos los módulos comparten las
mismas listas durante esa sesión. Los importes se presentan en colones (CRC)
como precios finales de demostración, sin cálculo fiscal. Tarjeta y transferencia
solo registran el medio de pago; no realizan cobros electrónicos.

## Estructura

- `src/SistemaVentasTienda.Core`: modelos, datos y reglas del sistema.
- `src/SistemaVentasTienda.WinForms`: formularios, controles y eventos.
- `tests/SistemaVentasTienda.Pruebas`: comprobaciones de la lógica.
- `docs`: propuesta y documentación técnica en Markdown.
- `.github/workflows/verificar.yml`: compilación y pruebas en un runner de Windows.

Los archivos `FrmNombre.Designer.cs` contienen la distribución de controles;
`FrmNombre.cs` contiene los eventos. Las validaciones y los cálculos pertenecen
a los servicios del proyecto Core. Empiecen la lectura por `Program.cs`, luego
`FrmPrincipal.cs` y después el servicio del módulo que van a modificar.

## Ejecución en Visual Studio

1. Extraiga **todo** el ZIP a una carpeta, por ejemplo `C:\Proyectos\SistemaVentasTienda`.
2. En Visual Studio Installer, habilite **Desarrollo de escritorio con .NET** y
   el SDK de .NET 8. Los proyectos utilizan .NET, no .NET Framework.
3. Abra `SistemaVentasTienda.sln`.
4. En el Explorador de soluciones, haga clic derecho en
   **SistemaVentasTienda.WinForms** y elija **Establecer como proyecto de inicio**.
   `Core` es una biblioteca y `Pruebas` es una aplicación de consola.
5. Seleccione **Compilar > Compilar solución** y luego presione **F5**.
6. Inicie con **Productos** o **Nueva venta**. El recorrido de ejemplo está en
   [el manual](docs/manual-usuario.md).

Si no aparece .NET 8, agregue el SDK desde Visual Studio Installer o desde
[las descargas oficiales](https://dotnet.microsoft.com/download/dotnet/8.0).
La [guía de Microsoft](https://learn.microsoft.com/dotnet/core/install/windows)
detalla la compatibilidad entre el SDK y Visual Studio.

## Ejecución desde una terminal

Abra PowerShell en la carpeta donde está la solución. El SDK debe estar instalado:

```powershell
dotnet build SistemaVentasTienda.sln --configuration Release -m:1 -p:TreatWarningsAsErrors=true
dotnet run --project src/SistemaVentasTienda.WinForms --configuration Release --no-build
```

La interfaz requiere Windows. Las pruebas de Core también pueden ejecutarse
desde Linux o macOS:

```powershell
dotnet run --project tests/SistemaVentasTienda.Pruebas --configuration Release
```

El resultado esperado es `38 aprobadas; 0 fallidas`. Estas pruebas son una consola
sin dependencias externas: se ejecutan con `dotnet run`, no con `dotnet test`.

## Trabajo en pareja

Antes de publicar, completen los nombres en este archivo y en `docs/propuesta.md`.
Sigan [la guía de Git y GitHub](docs/trabajo-en-pareja.md) para conservar el historial,
subir el proyecto, crear ramas y revisar los aportes de ambos.

El repositorio local se inició antes de implementar los módulos. Los commits
incluidos registran etapas reales de preparación con la identidad
`Preparacion asistida`. **No acreditan aportes de los integrantes**. Cada persona
deberá realizar, probar y registrar sus cambios con su propia cuenta. No se ha
creado ni publicado un repositorio en GitHub.

## Documentación

- [Propuesta y distribución inicial de tareas](docs/propuesta.md).
- [Arquitectura y reglas del sistema](docs/arquitectura.md).
- [Manual de uso](docs/manual-usuario.md).
- [Trabajo en pareja con GitHub](docs/trabajo-en-pareja.md).
- [Correspondencia con los requisitos del PDF](docs/requisitos.md).
- [Verificación y recorrido manual pendiente](docs/verificacion.md).
