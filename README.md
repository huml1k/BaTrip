# BaTrip

Desktop + gRPC проект для авторизации и базовой логики путешествий.

## Структура решения

- `BaTrip/BaTrip.Client` - Avalonia desktop клиент
- `BaTrip/BaTrip.Server` - ASP.NET Core gRPC сервер
- `BaTrip/BaTrip.Infrastructure` - EF Core, PostgreSQL, Redis, миграции
- `BaTrip/BaTrip.Domain` - доменные сущности и интерфейсы
- `BaTrip/BaTrip.Contracts` - `.proto` контракты gRPC

## Требования

- .NET SDK 10
- Docker Desktop (для PostgreSQL и Redis)
- (опционально) `dotnet-ef` для миграций

Проверка:

```powershell
dotnet --version
docker --version
```

## Быстрый старт

Ниже команды выполняются из папки `BaTrip` (где лежит `docker-compose.yml`):

```powershell
cd .\BaTrip
```

### 1) Поднять инфраструктуру

```powershell
docker compose up -d
```

Что поднимается:

- Redis: `localhost:6379`
- Redis Commander UI: <http://localhost:8082>

Проверка:

```powershell
docker ps
```

### 2) Настроить строку подключения сервера

Файл: `BaTrip.Server/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=host;Port=port;Database=db;Username=postgres;Password=pass",
  "Redis": "localhost:6379"
}
```

### 3) Применить миграции


Применить существующие миграции:

```powershell
dotnet ef database update --project BaTrip.Infrastructure --startup-project BaTrip.Server
```

Если нужно создать новую миграцию:

```powershell
dotnet ef migrations add <MigrationName> --project BaTrip.Infrastructure --startup-project BaTrip.Server --output-dir Migrations
dotnet ef database update --project BaTrip.Infrastructure --startup-project BaTrip.Server
```

### 4) Запустить сервер

```powershell
dotnet run --project BaTrip.Server
```

Сервер слушает gRPC endpoints:

- `https://localhost:7170` (HTTP/2)
- `http://localhost:5039` (HTTP/2)

### 5) Запустить клиент

В новом терминале:

```powershell
dotnet run --project BaTrip.Client
```

По умолчанию клиент подключается к `https://localhost:7170`.