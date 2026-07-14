# Kafe Yana

Sistema web de gestión para cafetería: inventario, recetas y costos, punto de venta (POS), compras, caja, fidelización de clientes y facturación electrónica (SIAT Bolivia).

## ¿Qué hace el sistema?

- **Inventario**: productos elaborados (con variaciones, recetas e insumos), productos comprados y productos combo.
- **Recetas y costos**: cálculo de márgenes y costos de producción (módulo terminado).
- **Ventas / POS**: flujo de punto de venta, mesas, rondas de pedidos, sub-ventas.
- **Compras**: órdenes de compra, proveedores, hitos de compra.
- **Caja**: apertura/cierre de caja.
- **Clientes y fidelización**: puntos, referidos, promociones, productos canjeables.
- **Facturación electrónica**: integración con el servicio SIAT (impuestos Bolivia), notas de ajuste, contingencia.
- **Impresoras**: impresión de tickets/comandas (cocina, barra, principal) vía red.
- **Reportes**: en desarrollo.

### Roles y permisos

- **admin**: acceso total a todos los módulos.
- **mesero**: solo módulo de ventas → punto de venta.
- **cajero**: punto de venta, ventas (historial), clientes, fidelización, caja.

### Estado de módulos

| Módulo | Estado |
|---|---|
| Dashboard | No terminado |
| Inventario | No terminado (falta kardex) |
| Ventas | No terminado |
| Compras | No terminado |
| Caja | No terminado |
| Reportes | No iniciado |
| Recetas y Costos | Terminado |
| Configuración | No terminado |

## Stack técnico

**Backend** (`/backend`)
- .NET 9 (ASP.NET Core), arquitectura en capas: `KafeYana.Api`, `KafeYana.Application`, `KafeYana.Domain` (Core), `KafeYana.Infrastructure`.
- HotChocolate (GraphQL) para queries GET.
- API REST (controllers) para POST/PUT/DELETE.
- Entity Framework Core + PostgreSQL (Npgsql).
- SignalR (hubs) para tiempo real.
- Autenticación con cookies + JWT.
- Cloudflare R2 para almacenamiento de archivos.
- QuestPDF para generación de reportes/documentos PDF.
- Integración con servicios SIAT (facturación electrónica Bolivia).

**Frontend** (`/frontend`)
- React 19 + TypeScript + Vite.
- Tailwind CSS.
- React Router.
- Peticiones GET vía GraphQL (`src/lib/graphql.ts`, queries en `src/lib/queries/[modulo].queries.ts`), peticiones POST/PUT/DELETE vía REST (`src/lib/api.ts`).
- SignalR client para tiempo real.
- Recharts (gráficos), jsPDF (PDFs), qrcode.react.

## Estructura del proyecto

```
backend/
  KafeYana.Api/
    KafeYana.Api/            → controllers, GraphQL, hubs, middlewares, Program.cs
    KafeYana.Application/    → interfaces de servicios
    KafeYana.Core/           → entidades de dominio, tipos de datos
    KafeYana.Domain/         → lógica de aplicación, DTOs, interfaces de repositorio
    KafeYana.Infrastructure/ → EF Core, migraciones, servicios, cliente SIAT
    KafeYana.IntegrationTests/
  Dockerfile

frontend/
  src/
    components/  → ui/, layout/, modals/, [feature]/
    contexts/    → React Context providers
    hooks/       → custom hooks
    lib/         → queries GraphQL, api.ts, lógica de negocio
    pages/       → auth, cash, fidelizacion, inventory, purchases, recipes, reports, sales, settings
    types/
    utils/
```

## Cómo levantar el proyecto

### Requisitos

- .NET SDK 9.0
- Node.js + pnpm
- PostgreSQL (base de datos local o remota)

### Backend

```bash
cd backend/KafeYana.Api/KafeYana.Api
```

Configurar `appsettings.Development.json` (o usar `dotnet user-secrets`) con al menos:

```json
{
  "DataBase": { "Conexion": "<cadena de conexión PostgreSQL>" },
  "JwtOptions": { "Secret": "<clave secreta>" }
}
```

Ejecutar:

```bash
dotnet restore
dotnet ef database update   # aplicar migraciones (si aplica)
dotnet run
```

La API queda disponible por defecto en `http://localhost:5012` (ajustar según `launchSettings.json`).

### Frontend

```bash
cd frontend
pnpm install
pnpm dev
```

Variables de entorno relevantes (`.env.development`):

```
VITE_API_URL=/api
VITE_GQL_URL=/graphql
VITE_COOKIE_DOMAIN=localhost
VITE_BACKEND_PROXY=http://localhost:5012
```

El frontend corre en `http://localhost:5173` (Vite) y proxya `/api` y `/graphql` hacia el backend.

### Docker (backend)

```bash
cd backend
docker build -t kafeyana-api .
docker run -p 8080:8080 --env-file .env kafeyana-api
```

## Convenciones de desarrollo (frontend)

- Un componente = una responsabilidad (UI). No hace fetch ni lógica de negocio.
- Hooks manejan estado/efectos, no retornan JSX.
- `lib/` contiene lógica pura (cálculos, mappers, helpers de negocio), sin importar React.
- Componentes de más de ~150 líneas deben dividirse.
- Ejecutar `pnpm build` (o `npm run build`) para validar tipos antes de dar por terminado un cambio.
