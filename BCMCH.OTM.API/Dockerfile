# FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS build

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build


EXPOSE 80
EXPOSE 8080
EXPOSE 1433
EXPOSE 15422
EXPOSE 5000
EXPOSE 5001

WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY BCMCH.OTM.API/*.csproj ./BCMCH.OTM.API/
COPY BCMCH.OTM.API.Shared/*.csproj ./BCMCH.OTM.API.Shared/
COPY BCMCH.OTM.Data/*.csproj ./BCMCH.OTM.Data/
COPY BCMCH.OTM.Data.Contract/*.csproj ./BCMCH.OTM.Data.Contract/
COPY BCMCH.OTM.Domain/*.csproj ./BCMCH.OTM.Domain/
COPY BCMCH.OTM.Domain.Contract/*.csproj ./BCMCH.OTM.Domain.Contract/
COPY BCMCH.OTM.Infrastucture/*.csproj ./BCMCH.OTM.Infrastucture/
COPY BCMCH.OTM.Infrastucture.IOC/*.csproj ./BCMCH.OTM.Infrastucture.IOC/
COPY BCMCH.OTM.Infrastucture.IOC/*.csproj ./BCMCH.OTM.Infrastucture.IOC/
COPY BCMCH.OTM.External/*.csproj ./BCMCH.OTM.External/

#
RUN dotnet restore 
#

# copy everything else and build app
# COPY BCMCH.OTM.API/. ./BCMCH.OTM.API/
# COPY BCMCH.OTM.API.Shared/. ./BCMCH.OTM.API.Shared/
# COPY BCMCH.OTM.Data/. ./BCMCH.OTM.Data/ 
# COPY BCMCH.OTM.Data.Contract/. ./BCMCH.OTM.Data.Contract/ 
# COPY BCMCH.OTM.Domain/. ./BCMCH.OTM.Domain/ 
# COPY BCMCH.OTM.Domain.Contract/. ./BCMCH.OTM.Domain.Contract/ 
# COPY BCMCH.OTM.Infrastucture/. ./BCMCH.OTM.Infrastucture/ 
# COPY BCMCH.OTM.Infrastucture.IOC/*.csproj ./BCMCH.OTM.Infrastucture.IOC/
copy . .
#


WORKDIR /app/BCMCH.OTM.API
RUN dotnet publish -c Release -o out
#
# FROM mcr.microsoft.com/dotnet/core/aspnet:6.0 AS runtime
# FROM mcr.microsoft.com/dotnet/runtime:6.0 AS runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic AS base

WORKDIR /app 

#
# RUN EXPORT ASPNETCORE_ENVIRONMENT="Development"
COPY --from=build /app/BCMCH.OTM.API/out ./
ENTRYPOINT ["dotnet", "BCMCH.OTM.API.dll"]

# ssh -i "bch.pem" ubuntu@ec2-54-227-68-105.compute-1.amazonaws.com
# sudo scp -r -i  "test.pem" BCH-OTM/BCMCH.OTM.API/backend.zip   ubuntu@ec2-100-25-13-139.compute-1.amazonaws.com:
# sudo docker build -f Dockerfile -t the_experiment .
# sudo docker run -p 5001:80 --name the_experiment_container the_experiment
# sudo docker rm -f 07f1cd77badea32ecadf29179813d4682b1baf4e68fd0141eb16238e19bcddfd
