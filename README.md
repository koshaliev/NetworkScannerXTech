# NetworkScannerXTech - сканер WiFi сетей
Для сканирования используется библиотека NativeManagedWifi

В качестве базы данных - PostgreSQL

# Установка
## Установка .NET 8 SDK
Проект собран на [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0), поэтому необходимо его установить.

## 2. Строка подключения к базе данных
Заменить значение у ключа `DefaultConnection` в файле `appsettings.json` на свою строку подключения к базе данных PostgreSQL.
```
{
  "ConnectionStrings": {
      "DefaultConnection": "ваша_строка_подключения_к_базе_данных_postgresql"
  }
}
```

## 3. Создание миграции
Для запуска необходимо создать и применить миграцию базы данных.

Командная строка/Powershell:
```
dotnet ef migrations add InitialCreate
```

или через Package Manager Console:

```
Add-Migration InitialCreate
```

## 4. Обновление базы данных
Командная строка/Powershell:
```
dotnet ef database update
```

или через Package Manager Console:

```
Update-Database
```
