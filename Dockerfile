FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserManipulations/UserManipulations.csproj", "UserManipulations/"]
COPY ["Models/Models.csproj", "Models/"]
RUN dotnet restore "UserManipulations/UserManipulations.csproj"
COPY . .
WORKDIR "/src/UserManipulations"
RUN dotnet build "UserManipulations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserManipulations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManipulations.dll"]