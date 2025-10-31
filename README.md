# DodoPizza API

Проект ASP.NET Core Web API для управления пиццерией с функционалом аутентификации, меню и корзины покупок.

## Функциональность

### Аутентификация и Регистрация
- **POST /pizza/authorization** - вход в систему с получением JWT токена
- **POST /pizza/register** - регистрация нового пользователя

### Меню
- **GET /pizza/menu** - получение списка всех позиций меню

### Корзина покупок
- **GET /pizza/basket/{id}** - просмотр содержимого корзины (требует авторизации)
- **POST /pizza/basket/add** - добавление товара в корзину (требует авторизации)
- **DELETE /pizza/basket/delete/{id}** - удаление товара из корзины (требует авторизации)

## Архитектура

Проект построен по принципам Clean Architecture с использованием:
- **MediatR** для реализации CQRS
- **Entity Framework Core** для работы с базой данных  
- **JWT** для аутентификации
- **Minimal API** для эндпоинтов
- **Docker** для контейнеризации

## База данных

Используется MySQL с следующими основными таблицами:
- **Users** - пользователи системы
- **Baskets** - корзины пользователей
- **BasketItems** - товары в корзинах
- **Menus** - меню пиццерии

## Безопасность

### Хеширование паролей
- Используется алгоритм PBKDF2 с SHA256
- Размер соли: 128 бит
- Размер ключа: 256 бит
- Количество итераций: 100,000
- Защита от timing-атак через `CryptographicOperations.FixedTimeEquals`

### JWT Токены
- Алгоритм подписи: HMAC SHA256
- Включает claims: UserId, Name, Email
- Настраиваемое время жизни токена

## Docker

Проект включает docker-compose для простого развертывания:
- **MySQL 9.4.0** - база данных
- **ASP.NET Core приложение** - основное API

### Запуск с Docker:
```bash
docker-compose up -d
```
## Настройка
**Конфигурация JWT**:
- **В appsettings.json необходимо указать настройки JWT:**
  ```bash
  {
  "Jwt": {
    "Key": "your-secret-key-here-minimum-32-characters",
    "Issuer": "dodopizza",
    "Audience": "dodopizza-users",
    "ExpireMinutes": 60
  }
  }
  ```
## Строка подключения к БД:
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=mysql;Port=3306;Database=PizzaDb;User=root;Password=rootpassword;"
  }
}
```
## WPF Клиент
**Приложение включает полнофункциональный WPF клиент с окнами:**

- AuthWindow - окно авторизации
- RegistrationWindow - окно регистрации
- MainWindow - главное окно с меню
- ChekBasketWindow - окно корзины покупок
- Функции клиента:
- Просмотр меню пиццерии
- Добавление товаров в корзину
- Просмотр и управление корзиной
- JWT аутентификация
- Автоматическое обновление токенов

## Запуск проекта
**Docker**
```bash
# Запуск всех сервисов
docker-compose up -d

# Остановка
docker-compose down
Вариант 2: Локальная разработка
bash
# Запуск базы данных
docker-compose up mysql -d

# Запуск приложения
dotnet run
```
# Запуск WPF клиента
```bash
./PizzaProject/bin/Debug/PizzaProject.exe
```

## Технологии
- **Backend**: ASP.NET Core 8.0, Entity Framework Core, MediatR
- **Database**: MySQL 9.4.0
- **Authentication**: JWT Bearer Tokens
- **Frontend**: WPF (.NET 6+)
- **Containerization**: Docker, Docker Compose
- **Security**: PBKDF2 хеширование, JWT

## API Документация
После запуска в режиме разработки доступна OpenAPI документация по адресу:

```text
http://localhost:8080/openapi/v1.json
```
### Важные заметки
- Для продакшн использования обязательно измените JWT секретный ключ
- Рекомендуется вынести чувствительные данные в переменные окружения
- Убедитесь, что порты 8080 (API) и 3306 (MySQL) свободны
- WPF клиент настроен на подключение к http://localhost:8080

## Разработка
**Проект использует Feature-based структуру для лучшей организации кода. Каждая функциональность содержит:**
- Handler (обработчик)
- Request/Response модели
- Endpoint (эндпоинт API)

## Авторство 
**Данияр Камаев, Ufa-Dynamics**

  
