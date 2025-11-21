# ---------------------------
# STAGE 1 - BUILD
# ---------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar todo el repo (porque tu proyecto está dentro de /ProyectoBilaps/src/)
COPY . .

# Restaurar dependencias del proyecto principal
RUN dotnet restore "ProyectoBilaps/src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj"

# Publicar en modo Release
RUN dotnet publish "ProyectoBilaps/src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj" \
    -c Release -o /out

# ---------------------------
# STAGE 2 - RUNTIME
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /out .

# Render usa el puerto 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProyectoBilaps.Presentation.dll"]
