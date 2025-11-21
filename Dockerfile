# STAGE 1 - BUILD
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los csproj de cada capa
COPY src/ProyectoBilaps.Application/ProyectoBilaps.Application.csproj ProyectoBilaps.Application/
COPY src/ProyectoBilaps.Domain/ProyectoBilaps.Domain.csproj ProyectoBilaps.Domain/
COPY src/ProyectoBilaps.Infrastructure/ProyectoBilaps.Infrastructure.csproj ProyectoBilaps.Infrastructure/
COPY src/ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj ProyectoBilaps.Presentation/

RUN dotnet restore ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj

# Copiar TODA la carpeta src
COPY src/ .

# Publicar
RUN dotnet publish ProyectoBilaps.Presentation/ProyectoBilaps.Presentation.csproj -c Release -o /app/publish

# STAGE 2 - FINAL IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProyectoBilaps.Presentation.dll"]
