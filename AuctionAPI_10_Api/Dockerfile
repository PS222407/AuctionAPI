#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AuctionAPI_10_Api/AuctionAPI_10_Api.csproj", "AuctionAPI_10_Api/"]
COPY ["AuctionAPI_20_BusinessLogic/AuctionAPI_20_BusinessLogic.csproj", "AuctionAPI_20_BusinessLogic/"]
COPY ["AuctionAPI_30_DataAccess/AuctionAPI_30_DataAccess.csproj", "AuctionAPI_30_DataAccess/"]
RUN dotnet restore "./AuctionAPI_10_Api/AuctionAPI_10_Api.csproj"
COPY . .
WORKDIR "/src/AuctionAPI_10_Api"
RUN dotnet build "./AuctionAPI_10_Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AuctionAPI_10_Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionAPI_10_Api.dll"]