# HorizonFutureVest

Aplicación ASP.NET Core MVC (Razor Views) + EF Core diseñada para que un analista ingrese indicadores macroeconómicos por país y pueda obtener un ranking con un scoring normalizado y la tasa de retorno estimada.

### Tecnología
- .NET 9
- ASP.NET Core MVC (Razor Views)
- Entity Framework Core (Code First)
- Bootstrap para UI

### Estructura principal
- `HorizonFutureVest/` — Proyecto web (UI, controladores, vistas, configuración). Ver `appsettings.json` aquí.
- `Application/` — Lógica de negocio, servicios, DTOs y ViewModels.
- `Persistence/` — Entidades, `DbContext`, configuraciones EF Core y repositorios.

### Requisitos
- .NET 9 SDK instalado
- SQL Server (o proveedor compatible) para la base de datos
- Visual Studio 2022/2023 o VS Code

### Configuración de la base de datos (IMPORTANTE)
Si alguien clona este repositorio y quiere ejecutar el proyecto localmente, debe actualizar la cadena de conexión en el archivo `appsettings.json` del proyecto web (`HorizonFutureVest/appsettings.json`).

- Localiza la clave `DefaultConnection` y reemplaza su valor por tu cadena de conexión a la base de datos.

Ejemplo (resumen):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=HorizonFutureVestDb;Trusted_Connection=True;"
}
```

Sin una cadena válida en `DefaultConnection` la aplicación no podrá conectarse a la base de datos y las funciones que dependen de EF Core fallarán.

### Migraciones y creación de la base de datos
Desde la carpeta del proyecto web o del proyecto `Persistence` (según tu flujo), puedes aplicar migraciones:

- Añadir migración: `dotnet ef migrations add NombreMigracion --project Persistence --startup-project HorizonFutureVest`
- Actualizar la base de datos: `dotnet ef database update --project Persistence --startup-project HorizonFutureVest`

(Ajusta `--project` y `--startup-project` según tu estructura y tu herramienta favorita.)