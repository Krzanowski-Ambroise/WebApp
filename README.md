New Project Objectif Mobility Pro c# ASP.NET 2 Months for 20/20 !

## Configuration

No secret is stored in the repository. `appsettings.json` keeps `Geoapify:ApiKey` empty; the values come from
[user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development or from environment variables.
`secrets.example.json` lists the expected keys (placeholders only).

| Key | Environment variable | Used for |
| --- | --- | --- |
| `Geoapify:ApiKey` | `Geoapify__ApiKey` | Geocoding of sites and job post distances ([Geoapify](https://www.geoapify.com/)) |
| `SeedAdmin:Email` | `SeedAdmin__Email` | Email of the admin account created at startup |
| `SeedAdmin:Password` | `SeedAdmin__Password` | Password of that admin account |

```bash
cd WebApp
dotnet user-secrets set "Geoapify:ApiKey" "<key>"
dotnet user-secrets set "SeedAdmin:Email" "<admin-email>"
dotnet user-secrets set "SeedAdmin:Password" "<admin-password>"
```

If `SeedAdmin:Email` or `SeedAdmin:Password` is missing, no admin account is created (a warning is logged).
`SeedAdmin:Password` must follow the Identity rules (at least 6 characters, with a lowercase letter, a digit and a
non-alphanumeric character), otherwise the account is not created.
The `WebAppConnection` connection string uses Windows authentication (`Trusted_Connection`); a connection string
with a password belongs in user-secrets or `ConnectionStrings__WebAppConnection`, never in `appsettings.json`.
