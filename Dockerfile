# Imagen base para construir (SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos .csproj de cada capa
COPY ProyectoBilaps/src/ProyectoBilaps.Domain/ProyectoBilaps.Domain.csproj ProyectoBilaps.Domain/
COPY ProyectoBilaps/src/ProyectoBilaps.Application/ProyectoBilaps.Application.csproj ProyectoBilaps.Application/
COPY ProyectoBilaps/src/ProyectoBilaps.Infrastructure/ProyectoBilaps.Infrastructure.csproj ProyectoBilaps.Infrastructure/
COPY ProyectoBilaps/src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj ProyectoBilaps.Presentation/

# Restaurar dependencias
RUN dotnet restore ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj

# Copiar todo el código
COPY ProyectoBilaps/src/ .

# Publicar la aplicación
RUN dotnet publish ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj -c Release -o /app/publish

# Imagen final de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProyectoBilaps.Presentation.dll"]
