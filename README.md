# [bolero](https://fsbolero.io//) example

## Build

dotnet core: 3.0.100
- Microsoft.AspNetCore.Blazor.Build: 3.0.0-preview9.19465.2
- Microsoft.AspNetCore.Blazor.DevServer: 3.0.0-preview9.19465.2
- Microsoft.AspNetCore.Blazor.Server: 3.0.0-preview9.19465.2
- Microsoft.EntityFrameworkCore: 3.0.0
- Microsoft.EntityFrameworkCore.Sqlite: 3.0.0

- Bolero: 0.10.1-preview9
- Bolero.Build: 0.10.1-preview9

```bash
dotnet build
```

## run

```base
sqlite3 App.Server/Data/Development.sq3 < App.Server/Data/default.sql

dotnet run -p App.Server
```

access: http://localhost:5000/