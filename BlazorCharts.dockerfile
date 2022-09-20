# syntax=docker/dockerfile:1.4
ARG BUILD_CONFIG=Release
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIG

COPY src/SensorsAPI/Client/*.csproj            /src/SensorsAPI/Client/
COPY src/BlazorCharts/Client/*.csproj   /src/BlazorCharts/Client/
COPY src/BlazorCharts/Server/*.csproj   /src/BlazorCharts/Server/
COPY src/BlazorCharts/Shared/*.csproj   /src/BlazorCharts/Shared/
COPY src/BlazorCharts/*.sln             /src/BlazorCharts/
RUN dotnet restore /src/BlazorCharts/

COPY src/SensorsAPI/Client/         /src/SensorsAPI/Client/
COPY src/BlazorCharts/Client /src/BlazorCharts/Client/
COPY src/BlazorCharts/Server /src/BlazorCharts/Server/
COPY src/BlazorCharts/Shared /src/BlazorCharts/Shared/
WORKDIR /src/BlazorCharts/Server/
RUN dotnet publish --no-restore -c ${BUILD_CONFIG} -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
ARG BUILD_CONFIG
WORKDIR /app

# Install debugger for debug configurations
RUN test $BUILD_CONFIG = "Debug" && \
    apt-get update && \
    apt-get -y install curl procps && \
    curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg


COPY --from=build /app /app
ENTRYPOINT ["dotnet", "BlazorCharts.Server.dll"]
EXPOSE 8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS='http://+:8080'
ENV ASPNETCORE_ENVIRONMENT='Production'
ENV Logging__Console__FormatterName='Simple'
