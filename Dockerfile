# ---------------------------
# STAGE 1 - BUILD
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos los csproj de cada capa
COPY ProyectoBilaps/src/ProyectoBilaps.Domain/ProyectoBilaps.Domain.csproj ProyectoBilaps.Domain/
COPY ProyectoBilaps/src/ProyectoBilaps.Application/ProyectoBilaps.Application.csproj ProyectoBilaps.Application/
COPY ProyectoBilaps/src/ProyectoBilaps.Infrastructure/ProyectoBilaps.Infrastructure.csproj ProyectoBilaps.Infrastructure/
COPY ProyectoBilaps/src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj ProyectoBilaps.Presentation/

# Restaurar paquetes
RUN dotnet restore ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj

# Copiar el resto del proyecto
COPY ProyectoBilaps/src/ ProyectoBilaps/

# Publicar
RUN dotnet publish ProyectoBilaps/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj \
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
