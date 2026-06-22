FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Cart.Api/Cart.Api.csproj", "Api/Cart.Api/"]
COPY ["Application/Cart.Application/Cart.Application.csproj", "Application/Cart.Application/"]
COPY ["Domain/Model/Model.csproj", "Domain/Model/"]
COPY ["Application/Product.Application/Product.Application.csproj", "Application/Product.Application/"]
COPY ["Infrastructure/Cart.Infrastructure/Cart.Infrastructure.csproj", "Infrastructure/Cart.Infrastructure/"]
COPY ["Infrastructure/Shared.Infrastructure/Shared.Infrastructure.csproj", "Infrastructure/Shared.Infrastructure/"]
RUN dotnet restore "Api/Cart.Api/Cart.Api.csproj"
COPY . .
WORKDIR "/src/Api/Cart.Api"
RUN dotnet build "Cart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Cart.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cart.Api.dll"]
