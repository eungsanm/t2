Migrar y Actualizar db previamente creada:

dotnet ef migrations add InitialCreate --project src/t2.Infrastructure --startup-project src/t2.API

dotnet ef database update --project src/t2.Infrastructure --startup-project src/t2.API
