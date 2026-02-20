# HR CRM — Управление персоналом

Веб-приложение для отдела кадров на ASP.NET MVC (.NET 8) + PostgreSQL.

## Возможности

- **Сотрудники** — полный CRUD, поиск по ФИО/email, фильтрация по отделу, статус (работает/уволен)
- **Отделы** — создание, удаление (с защитой от удаления непустых отделов), счётчик сотрудников
- **Должности** — создание, удаление, базовый оклад, счётчик сотрудников
- **Дашборд** — карточки со статистикой: всего сотрудников, активных, количество отделов

## Стек технологий

| Компонент   | Технология                           |
| ----------- | ------------------------------------ |
| Фреймворк   | ASP.NET MVC (.NET 8)                 |
| ORM         | Entity Framework Core 8              |
| База данных | PostgreSQL (Npgsql)                  |
| UI          | Bootstrap 5.3 + Bootstrap Icons      |
| Валидация   | Data Annotations + jQuery Validation |

## Структура проекта

```
hr-crm/
├── HrCrm.sln                          # Solution файл
├── setup.sh                            # Скрипт первоначальной настройки
├── .gitignore
└── HrCrm/
    ├── Program.cs                      # Точка входа, DI, маршрутизация
    ├── appsettings.json                # Строка подключения к БД
    ├── HrCrm.csproj                    # Зависимости проекта
    ├── Data/
    │   └── AppDbContext.cs             # EF Core контекст + seed-данные
    ├── Models/
    │   ├── Employee.cs                 # Сотрудник (ФИО, контакты, даты, отдел, должность)
    │   ├── Department.cs               # Отдел (название, описание)
    │   └── Position.cs                 # Должность (название, оклад)
    ├── Controllers/
    │   ├── HomeController.cs           # Дашборд со статистикой
    │   ├── EmployeesController.cs      # CRUD сотрудников + поиск/фильтрация
    │   ├── DepartmentsController.cs    # CRUD отделов
    │   └── PositionsController.cs      # CRUD должностей
    └── Views/
        ├── _ViewImports.cshtml         # Глобальные using и TagHelpers
        ├── _ViewStart.cshtml           # Layout по умолчанию
        ├── Shared/
        │   ├── _Layout.cshtml          # Основной layout с боковым меню
        │   └── _ValidationScriptsPartial.cshtml
        ├── Home/
        │   └── Index.cshtml            # Дашборд
        ├── Employees/
        │   ├── Index.cshtml            # Таблица сотрудников
        │   ├── Create.cshtml           # Форма создания
        │   └── Edit.cshtml             # Форма редактирования
        ├── Departments/
        │   ├── Index.cshtml            # Карточки отделов
        │   └── Create.cshtml           # Форма создания
        └── Positions/
            ├── Index.cshtml            # Таблица должностей
            └── Create.cshtml           # Форма создания
```

## Модели данных

### Employee (Сотрудник)

| Поле            | Тип       | Описание                          |
| --------------- | --------- | --------------------------------- |
| Id              | int       | Первичный ключ                    |
| LastName        | string    | Фамилия (обязательно)             |
| FirstName       | string    | Имя (обязательно)                 |
| MiddleName      | string?   | Отчество                          |
| Email           | string?   | Электронная почта                 |
| Phone           | string?   | Телефон                           |
| BirthDate       | DateOnly? | Дата рождения                     |
| HireDate        | DateOnly  | Дата приёма на работу             |
| TerminationDate | DateOnly? | Дата увольнения (null = работает) |
| DepartmentId    | int?      | FK на отдел                       |
| PositionId      | int?      | FK на должность                   |

### Department (Отдел)

| Поле        | Тип     | Описание                                |
| ----------- | ------- | --------------------------------------- |
| Id          | int     | Первичный ключ                          |
| Name        | string  | Название (обязательно, до 200 символов) |
| Description | string? | Описание                                |

### Position (Должность)

| Поле       | Тип      | Описание                                          |
| ---------- | -------- | ------------------------------------------------- |
| Id         | int      | Первичный ключ                                    |
| Title      | string   | Название должности (обязательно, до 200 символов) |
| BaseSalary | decimal? | Базовый оклад в рублях                            |

## Установка и запуск

### Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)

### Быстрый старт

```bash
# Клонирование
git clone https://github.com/io777/hr-crm.git
cd hr-crm

# Автоматическая настройка (создаёт БД, миграцию, собирает проект)
./setup.sh

# Запуск
cd HrCrm
dotnet run
```

Приложение будет доступно на `http://localhost:5139`

### Ручная настройка

```bash
cd hr-crm/HrCrm

# Установка зависимостей
dotnet restore

# Установка EF Core CLI (если нет)
dotnet tool install --global dotnet-ef

# Создание базы данных
psql -U postgres -c "CREATE DATABASE hr_crm"

# Миграция
dotnet ef migrations add Init
dotnet ef database update

# Запуск
dotnet run
```

### Настройка подключения к БД

Отредактируй `HrCrm/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=hr_crm;Username=postgres;Password=postgres"
  }
}
```

Или через переменные окружения в `setup.sh`:

```bash
DB_HOST=myserver DB_PORT=5433 DB_USER=admin DB_PASS=secret ./setup.sh
```

## Seed-данные

При первой миграции автоматически создаются:

**Отделы:**

- Администрация
- Производство
- Продажи

**Должности:**

- Директор — 150 000 ₽
- Менеджер — 80 000 ₽
- Инженер — 100 000 ₽

## Скриншоты интерфейса

Приложение использует Bootstrap 5 с тёмным боковым меню:

- `/` — Дашборд с карточками статистики
- `/Employees` — Таблица сотрудников с поиском и фильтрацией
- `/Employees/Create` — Форма добавления сотрудника
- `/Employees/Edit/{id}` — Редактирование сотрудника
- `/Departments` — Карточки отделов с количеством сотрудников
- `/Positions` — Таблица должностей с окладами

## Бизнес-логика

- Нельзя удалить отдел или должность, если к ним привязаны сотрудники
- Сотрудник считается активным, если `TerminationDate == null`
- При старте приложения автоматически применяются миграции (`db.Database.Migrate()`)
- Все формы защищены `AntiForgeryToken`

## Лицензия

MIT
