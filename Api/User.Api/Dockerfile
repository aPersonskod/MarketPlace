FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/User.Api/User.Api.csproj", "Api/User.Api/"]
COPY ["Infrastructure/User.Infrastructure/User.Infrastructure.csproj", "Infrastructure/User.Infrastructure/"]
COPY ["Domain/Model/Model.csproj", "Domain/Model/"]
COPY ["Application/User.Application/User.Application.csproj", "Application/User.Application/"]
RUN dotnet restore "Api/User.Api/User.Api.csproj"
COPY . .
WORKDIR "/src/Api/User.Api"
RUN dotnet build "User.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "User.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Api.dll"]
