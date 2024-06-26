﻿[![Coverage Status](https://coveralls.io/repos/github/PS222407/AuctionAPI/badge.svg?branch=coverage)](https://coveralls.io/github/PS222407/AuctionAPI?branch=coverage)
# AuctionAPI

## Getting started
Create docker network named "auctionapi_network"
```bash
docker network create auctionapi_network
```
Add this network to the docker start script  
And add the following environment variable(s) to the docker start script  
```env
ConnectionStrings__DefaultConnection="Server=auctionapi-mysql;Port=3306;Database=auctionapi;User ID=user;Password=password;"
```  
Run the following command in the root folder (where the .sln is located) to start the docker containers
```bash
docker-compose up -d
```
To start the application container you can start it via Visual Studio

## migration commands
Add/change the connection string in the appsettings.json to:
```text
Server=localhost;Port=3306;Database=auctionapi;User ID=user;Password=password;
```
Run the following commands in the project folder AuctionAPI_10_Api
```bash
dotnet ef migrations add MIGRATION_NAME --project="../AuctionAPI_30_DataAccess"
```
```bash
dotnet ef database update --project="../AuctionAPI_30_DataAccess"
```

## ReSharper
Please run this command before pushing to the repository
```bash
jb cleanupcode --no-build .\AuctionAPI.sln -o="resharper_log.yml"
```

## Generate coverage reports
Run the following commands in the root folder (where the .sln is located) to generate the coverage files
```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory:"xplat"
```
```bash
mkdir -p coverage/1 \
mkdir -p coverage/2 \
mv ./xplat/*/coverage.cobertura.xml ./coverage/1 \
mv ./xplat/*/coverage.cobertura.xml ./coverage/2
```
This generates the html report in the folder coveragereport
```bash
reportgenerator -reports:".\coverage\1\coverage.cobertura.xml;.\coverage\2\coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:HtmlInline
```
