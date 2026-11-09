# Trabajo en pareja con Git y GitHub

## Qué contiene la entrega

El ZIP conserva el repositorio `.git` de esta preparación y una copia portátil
`historial_inicial.bundle`. Los commits se realizaron mientras se construían las
etapas; no se reescribieron fechas ni se atribuyeron cambios a estudiantes.

El PDF exige un repositorio desde el inicio, participación de ambos, evolución,
correcciones y actualización continua de documentos. Este historial registra la
preparación local. **No sustituye el desarrollo real de la pareja en GitHub ni
garantiza por sí solo el cumplimiento de ese criterio.** Acuerden con su docente
cómo incorporar esta base a su proceso de trabajo y documenten sus propios aportes.

## Revisar y publicar la base

1. Completen los integrantes en `README.md` y `docs/propuesta.md`.
2. Cada uno debe configurar sus datos en su propia computadora. En PowerShell,
   dentro de la carpeta del proyecto:

   ```powershell
   git config user.name "Tu nombre"
   git config user.email "Tu correo asociado a GitHub"
   git status
   git log --oneline
   ```

3. Conserven y registren cualquier modificación real antes de subir:

   ```powershell
   git add README.md docs/propuesta.md
   git commit -m "Completar integrantes y acordar responsabilidades del avance"
   ```

   Ejecútenlo después de editar esos archivos; si no hay cambios, Git no creará
   un commit. No hace falta volver a ejecutar `git init`.

4. Un integrante crea en GitHub un repositorio **vacío**, sin README, licencia ni
   `.gitignore`, e invita al otro como colaborador. En la carpeta del proyecto
   compruebe `git remote -v`. Esta entrega no trae ningún remoto.
5. Sustituya `TU_USUARIO` y `TU_REPOSITORIO` por los datos reales y ejecute:

   ```powershell
   git remote add origin https://github.com/TU_USUARIO/TU_REPOSITORIO.git
   git push -u origin main
   ```

   GitHub pedirá autenticarse cuando corresponda. Si el remoto ya existe o no
   está vacío, revisen su contenido antes de integrar; eviten sobrescribirlo.

6. El segundo integrante clona **ese mismo repositorio remoto**:

   ```powershell
   git clone https://github.com/TU_USUARIO/TU_REPOSITORIO.git
   cd TU_REPOSITORIO
   ```

Si al extraer se perdió `.git`, conserve la carpeta original y recupere el
historial en una carpeta nueva desde el bundle incluido:

```powershell
git clone .\historial_inicial.bundle ..\SistemaVentasTienda-recuperado
cd ..\SistemaVentasTienda-recuperado
git remote remove origin
```

El remoto creado por ese clone apuntaba al bundle local. Después de quitarlo,
continúe con los pasos de configuración y publicación. El bundle corresponde
a la base de preparación; copie también cualquier cambio posterior que hayan
hecho en la otra carpeta, revise `git diff` y regístrelo.

## Flujo para cada tarea

1. Definan una mejora concreta y quién la implementará. Antes de empezar:

   ```powershell
   git switch main
   git pull --ff-only origin main
   git switch -c feature/nombre-de-la-tarea
   ```

2. Modifiquen un módulo, prueben su comportamiento y actualicen la documentación
   afectada. Mantengan las reglas en Core y los eventos en WinForms.
3. Revisen `git diff`; agreguen solamente los archivos de la tarea. Creen commits
   con mensajes que describan lo que realmente hicieron, por ejemplo:
   `Agregar filtro por categoría al inventario` o
   `Corregir actualización del resumen después de una venta`.
4. Publiquen su rama con `git push -u origin feature/nombre-de-la-tarea`.
5. Abran un Pull Request hacia `main`. El compañero revisa el código, ejecuta el
   recorrido y solicita las correcciones necesarias antes de integrarlo.
6. Una vez integrado, ambos actualizan su `main`. Dividan los cambios para
   evitar editar simultáneamente el mismo archivo `.Designer.cs`.

No creen commits vacíos ni cambien la autoría para simular participación.
Conserven los aportes de cada persona y la revisión cruzada de los PR.

## Convenciones de código

- Nombres en español que expresen la intención: `CalcularTotal`, `Existencias`.
- Clases y métodos con PascalCase; campos privados y variables con camelCase.
- Prefijos de controles: `txt`, `nud`, `cbo`, `dtp`, `dgv`, `btn`, `lbl`.
- Cuatro espacios de sangría en C#. Comentarios sobre propósito y decisiones.
- Dinero con `decimal`; cantidades con `int`; fechas con `DateTime`.
- Si una regla puede fallar, validarla antes de modificar las listas.
- No subir `bin`, `obj`, `.vs`, contraseñas ni archivos personales.

## Revisión automática

`.github/workflows/verificar.yml` compila y ejecuta las pruebas de lógica en un
runner Windows cuando se haga push o se abra un PR, si Actions está habilitado.
Su resultado se consulta en la pestaña **Actions** de GitHub. Está preparado,
pero todavía no se ha ejecutado remotamente. La revisión visual manual continúa
siendo necesaria.
