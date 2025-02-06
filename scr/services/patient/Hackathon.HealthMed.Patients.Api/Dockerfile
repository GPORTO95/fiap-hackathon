# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8084
ENV ASPNETCORE_URLS=http://0.0.0.0:8084
# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["scr/services/patient/Hackathon.HealthMed.Patients.Api/Hackathon.HealthMed.Patients.Api.csproj", "scr/services/patient/Hackathon.HealthMed.Patients.Api/"]
COPY ["scr/services/patient/Hackathon.HealthMed.Patients.Infrastructure/Hackathon.HealthMed.Patients.Infrastructure.csproj", "scr/services/patient/Hackathon.HealthMed.Patients.Infrastructure/"]
COPY ["scr/building blocks/Hackathon.HealthMed.Api.Core/Hackathon.HealthMed.Api.Core.csproj", "scr/building blocks/Hackathon.HealthMed.Api.Core/"]
COPY ["scr/building blocks/Hackathon.HealthMed.Kernel/Hackathon.HealthMed.Kernel.csproj", "scr/building blocks/Hackathon.HealthMed.Kernel/"]
COPY ["scr/services/patient/Hackathon.HealthMed.Patients.Application/Hackathon.HealthMed.Patients.Application.csproj", "scr/services/patient/Hackathon.HealthMed.Patients.Application/"]
COPY ["scr/services/patient/Hackathon.HealthMed.Patients.Domain/Hackathon.HealthMed.Patients.Domain.csproj", "scr/services/patient/Hackathon.HealthMed.Patients.Domain/"]

RUN dotnet restore "./scr/services/patient/Hackathon.HealthMed.Patients.Api/Hackathon.HealthMed.Patients.Api.csproj"
COPY . .

WORKDIR "/src/scr/services/patient/Hackathon.HealthMed.Patients.Api"
RUN dotnet build "./Hackathon.HealthMed.Patients.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hackathon.HealthMed.Patients.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hackathon.HealthMed.Patients.Api.dll"]