#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BuyRequestAPI/BuyRequestAPI.csproj", "BuyRequestAPI/"]
COPY ["BuyRequestApplication/BuyRequest.Application.csproj", "BuyRequestApplication/"]
COPY ["BuyRequestDomain/BuyRequest.Domain.csproj", "BuyRequestDomain/"]
COPY ["FinancialChallenge_Oscar.Infrastructure/FinancialChallenge_Oscar.Infrastructure.csproj", "FinancialChallenge_Oscar.Infrastructure/"]
COPY ["BankRequest.ClientApi/BankRequest.ClientApi.csproj", "BankRequest.ClientApi/"]
COPY ["BankRequestApplication/BankRequest.Application.csproj", "BankRequestApplication/"]
COPY ["BankRequestDomain/BankRequest.Domain.csproj", "BankRequestDomain/"]
COPY ["BankRequestData/BankRequest.Data.csproj", "BankRequestData/"]
COPY ["BuyRequestData/BuyRequest.Data.csproj", "BuyRequestData/"]
RUN dotnet restore "BuyRequestAPI/BuyRequestAPI.csproj"
COPY . .
WORKDIR "/src/BuyRequestAPI"
RUN dotnet build "BuyRequestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BuyRequestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BuyRequestAPI.dll"]