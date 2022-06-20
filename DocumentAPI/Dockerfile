#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DocumentAPI/DocumentAPI.csproj", "DocumentAPI/"]
COPY ["DocumentApplication/Document.Application.csproj", "DocumentApplication/"]
COPY ["BankRequest.ClientApi/BankRequest.ClientApi.csproj", "BankRequest.ClientApi/"]
COPY ["BankRequestApplication/BankRequest.Application.csproj", "BankRequestApplication/"]
COPY ["BankRequestDomain/BankRequest.Domain.csproj", "BankRequestDomain/"]
COPY ["FinancialChallenge_Oscar.Infrastructure/FinancialChallenge_Oscar.Infrastructure.csproj", "FinancialChallenge_Oscar.Infrastructure/"]
COPY ["BankRequestData/BankRequest.Data.csproj", "BankRequestData/"]
COPY ["DocumentData/Document.Data.csproj", "DocumentData/"]
COPY ["DocumentDomain/Document.Domain.csproj", "DocumentDomain/"]
RUN dotnet restore "DocumentAPI/DocumentAPI.csproj"
COPY . .
WORKDIR "/src/DocumentAPI"
RUN dotnet build "DocumentAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentAPI.dll"]