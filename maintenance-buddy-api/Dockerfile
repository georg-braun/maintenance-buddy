FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# copy code to the layer and restore the project.
# use specific sdk version, because in newer version there were some build problems.
FROM mcr.microsoft.com/dotnet/sdk:6.0@sha256:fde93347d1cc74a03f1804f113ce85add00c6f0af15881181165ef04bc76bd00 AS build
WORKDIR /src
COPY ["maintenance-buddy-api/maintenance-buddy-api.csproj", "maintenance-buddy-api/"]
RUN dotnet restore "maintenance-buddy-api/maintenance-buddy-api.csproj"
COPY . .

# run tests
RUN dotnet test

# build the project
WORKDIR "/src/maintenance-buddy-api"
RUN dotnet build "maintenance-buddy-api.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "maintenance-buddy-api.csproj" -c Release -o /app/publish

# final layer with the published software
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "maintenance-buddy-api.dll"]