FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["maintenance-buddy-api/maintenance-buddy-api.csproj", "maintenance-buddy-api/"]
RUN dotnet restore "maintenance-buddy-api/maintenance-buddy-api.csproj"
COPY . .
WORKDIR "/src/maintenance-buddy-api"
RUN dotnet build "maintenance-buddy-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "maintenance-buddy-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "maintenance-buddy-api.dll"]
