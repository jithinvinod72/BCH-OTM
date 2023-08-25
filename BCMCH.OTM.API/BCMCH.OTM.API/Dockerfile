FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY BCMCH.OTM.API/*.csproj ./BCMCH.OTM.API/
COPY BCMCH.OTM.API.Shared/*.csproj ./BCMCH.OTM.API.Shared/
COPY BCMCH.OTM.API.Data/*.csproj ./BCMCH.OTM.API.Data/
COPY BCMCH.OTM.API.Data.Contract/*.csproj ./BCMCH.OTM.API.Data.Contract/
COPY BCMCH.OTM.API.Domain/*.csproj ./BCMCH.OTM.API.Domain/
COPY BCMCH.OTM.API.Domain.Contract/*.csproj ./BCMCH.OTM.API.Domain.Contract/
COPY BCMCH.OTM.API.Infrastructure/*.csproj ./BCMCH.OTM.API.Infrastructure/
COPY BCMCH.OTM.API.Infrastructure.IOC/*.csproj ./BCMCH.OTM.API.Infrastructure.IOC/

#
RUN dotnet restore 
#
# copy everything else and build app
COPY BCMCH.OTM.API/. ./BCMCH.OTM.API/
COPY BCMCH.OTM.API.Shared/. ./BCMCH.OTM.API.Shared/
COPY BCMCH.OTM.API.Data/. ./BCMCH.OTM.API.Data/ 
COPY BCMCH.OTM.API.Data.Contract/. ./BCMCH.OTM.API.Data.Contract/ 
COPY BCMCH.OTM.API.Domain/. ./BCMCH.OTM.API.Domain/ 
COPY BCMCH.OTM.API.Domain.Contract/. ./BCMCH.OTM.API.Domain.Contract/ 
COPY BCMCH.OTM.API.Infrastructure/. ./BCMCH.OTM.API.Infrastructure/ 
COPY BCMCH.OTM.API.Infrastructure.IOC/*.csproj ./BCMCH.OTM.API.Infrastructure.IOC/
#
WORKDIR /app/BCMCH.OTM.API
RUN dotnet publish -c Release -o out 
#
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app 
#
COPY --from=build /app/BCMCH.OTM.API/out ./
ENTRYPOINT ["dotnet", "Experiment.dll"]