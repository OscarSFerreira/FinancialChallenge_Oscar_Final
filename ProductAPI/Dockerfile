#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ProductAPI/ProductAPI.csproj", "ProductAPI/"]
COPY ["Product.Application/Product.Application.csproj", "Product.Application/"]
COPY ["Product.Domain/Product.Domain.csproj", "Product.Domain/"]
COPY ["FinancialChallenge_Oscar.Infrastructure/FinancialChallenge_Oscar.Infrastructure.csproj", "FinancialChallenge_Oscar.Infrastructure/"]
COPY ["Product.Data/Product.Data.csproj", "Product.Data/"]
RUN dotnet restore "ProductAPI/ProductAPI.csproj"
COPY . .
WORKDIR "/src/ProductAPI"
RUN dotnet build "ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductAPI.dll"]