# [bolero](https://fsbolero.io//) example (WebAssembly)

## Build

dotnet core: 3.1.101
- Microsoft.AspNetCore.Blazor.Build: 3.1.0-preview4.19579.2
- Microsoft.AspNetCore.Blazor.DevServer: 3.1.0-preview4.19579.2
- Microsoft.AspNetCore.Blazor.Server: 3.1.0-preview4.19579.2
- Microsoft.EntityFrameworkCore: 3.1.0
- Microsoft.EntityFrameworkCore.Sqlite: 3.1.0

- Bolero: 0.11.21-preview31
- Bolero.Build: 0.11.21-preview31

```bash
dotnet build
```

## run

```base
sqlite3 App.Server/Data/Development.sq3 < App.Server/Data/default.sql

dotnet run -p App.Server
```

access: http://localhost:5000/