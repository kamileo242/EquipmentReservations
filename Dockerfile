FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/EquipmentReservations.WebApi/EquipmentReservations.WebApi.csproj EquipmentReservations.WebApi/
COPY src/EquipmentReservations.Domain/EquipmentReservations.Domain.csproj EquipmentReservations.Domain/
COPY src/EquipmentReservations.DataLayer/EquipmentReservations.DataLayer.csproj EquipmentReservations.DataLayer/
COPY src/EquipmentReservations.Models/EquipmentReservations.Models.csproj EquipmentReservations.Models/
COPY src/EquipmentReservations.RabbitMq/EquipmentReservations.RabbitMq.csproj EquipmentReservations.RabbitMq/

RUN dotnet restore EquipmentReservations.WebApi/EquipmentReservations.WebApi.csproj

COPY . .

RUN dotnet publish EquipmentReservations.WebApi/EquipmentReservations.WebApi.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "EquipmentReservations.WebApi.dll"]
