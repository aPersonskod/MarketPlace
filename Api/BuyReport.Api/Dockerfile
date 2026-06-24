FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/BuyReport.Api/BuyReport.Api.csproj", "Api/BuyReport.Api/"]
COPY ["Application/BuyReport.Application/BuyReport.Application.csproj", "Application/BuyReport.Application/"]
COPY ["Domain/Model/Model.csproj", "Domain/Model/"]
COPY ["Infrastructure/BuyReport.Infrastructure/BuyReport.Infrastructure.csproj", "Infrastructure/BuyReport.Infrastructure/"]
COPY ["Infrastructure/Shared.Infrastructure/Shared.Infrastructure.csproj", "Infrastructure/Shared.Infrastructure/"]
RUN dotnet restore "Api/BuyReport.Api/BuyReport.Api.csproj"
COPY . .
WORKDIR "/src/Api/BuyReport.Api"
RUN dotnet build "BuyReport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BuyReport.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BuyReport.Api.dll"]
