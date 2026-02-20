#!/bin/bash
set -e

echo "=== HR CRM Setup ==="

# Настройки БД (можно переопределить через переменные окружения)
DB_HOST="${DB_HOST:-localhost}"
DB_PORT="${DB_PORT:-5432}"
DB_NAME="${DB_NAME:-hr_crm}"
DB_USER="${DB_USER:-postgres}"
DB_PASS="${DB_PASS:-postgres}"

CONNECTION="Host=$DB_HOST;Port=$DB_PORT;Database=$DB_NAME;Username=$DB_USER;Password=$DB_PASS"

cd "$(dirname "$0")/HrCrm"

# 1. Restore пакетов
echo "→ Восстановление пакетов..."
dotnet restore

# 2. Установка dotnet-ef если нет
if ! dotnet ef --version &>/dev/null; then
    echo "→ Установка dotnet-ef..."
    dotnet tool install --global dotnet-ef
fi

# 3. Создание БД (если PostgreSQL доступен)
if command -v psql &>/dev/null; then
    echo "→ Создание базы данных $DB_NAME..."
    PGPASSWORD="$DB_PASS" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -tc \
        "SELECT 1 FROM pg_database WHERE datname = '$DB_NAME'" | grep -q 1 \
        || PGPASSWORD="$DB_PASS" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -c \
        "CREATE DATABASE $DB_NAME"
else
    echo "⚠ psql не найден — убедись, что база $DB_NAME создана вручную"
fi

# 4. Миграция
echo "→ Создание миграции..."
dotnet ef migrations add Init --no-build 2>/dev/null || dotnet ef migrations add Init

# 5. Применение миграции
echo "→ Применение миграции..."
dotnet ef database update

# 6. Сборка
echo "→ Сборка проекта..."
dotnet build

echo ""
echo "=== Готово! ==="
echo "Запуск: cd hr-crm/HrCrm && dotnet run"
echo "Приложение будет доступно на http://localhost:5139"
