FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

EXPOSE 5000

WORKDIR /app

COPY backlog.api/bin/Release/net6.0/publish .

ENV ASPNETCORE_URLS http://+:5000

ENTRYPOINT dotnet backlog.api.dll
