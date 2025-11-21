# ---------------------------
# STAGE 1 - BUILD
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los csproj según estructura del repositorio
COPY ProyectoBilaps/src/ProyectoBilaps.Domain/ProyectoBilaps.Domain.csproj ProyectoBilaps.Domain/
COPY ProyectoBilaps/src/ProyectoBilaps.Application/ProyectoBilaps.Application.csproj ProyectoBilaps.Application/
COPY ProyectoBilaps/src/ProyectoBilaps.Infrastructure/ProyectoBilaps.Infrastructure.csproj ProyectoBilaps.Infrastructure/
COPY ProyectoBilaps/src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj ProyectoBilaps.Presentation/

# Restaurar dependencias del proyecto principal
RUN dotnet restore "ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj"

# Copiar todo el código
COPY ProyectoBilaps/src/ .

# Publicar en modo Release
RUN dotnet publish ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj \
    -c Release -o /app/publish


# ---------------------------
# STAGE 2 - RUNTIME
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Render usa el puerto 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ProyectoBilaps.Presentation.dll"]
