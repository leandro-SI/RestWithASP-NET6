#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
# EXPOSE 443

# Set environment variable
ENV WEBSITED_PORT=8080
ENV DEBUG=true

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RestWithASPNET.API/RestWithASPNET.API.csproj", "RestWithASPNET.API/"]
RUN dotnet restore "./RestWithASPNET.API/RestWithASPNET.API.csproj"
COPY . .
WORKDIR "/src/RestWithASPNET.API"
RUN dotnet build "./RestWithASPNET.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RestWithASPNET.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestWithASPNET.API.dll"]