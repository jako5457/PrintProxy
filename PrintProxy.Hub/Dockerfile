# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt update && apt install curl -y
USER 0
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PrintProxy.Hub/PrintProxy.Hub.csproj", "PrintProxy.Hub/"]
COPY ["3DPrintLib/PrintLib.csproj", "3DPrintLib/"]
RUN dotnet restore "./PrintProxy.Hub/PrintProxy.Hub.csproj"
COPY . .
WORKDIR "/src/PrintProxy.Hub"
RUN dotnet build "./PrintProxy.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PrintProxy.Hub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
RUN apt update && apt install curl -y
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrintProxy.Hub.dll"]