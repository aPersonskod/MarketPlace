FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Product.Api/Product.Api.csproj", "Api/Product.Api/"]
COPY ["Application/Product.Application/Product.Application.csproj", "Application/Product.Application/"]
COPY ["Domain/Model/Model.csproj", "Domain/Model/"]
COPY ["Infrastructure/Product.Infrastructure/Product.Infrastructure.csproj", "Infrastructure/Product.Infrastructure/"]
RUN dotnet restore "Api/Product.Api/Product.Api.csproj"
COPY . .
WORKDIR "/src/Api/Product.Api"
RUN dotnet build "Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Product.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Product.Api.dll"]
