#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
ARG VERSION=8.0

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ./*.sln .
COPY ./JWTAuthentication.API/*.csproj JWTAuthentication.API/
COPY ./JWTAuthentication.Common/*.csproj JWTAuthentication.Common/
COPY ./JWTAuthentication.Domain/*.csproj JWTAuthentication.Domain/
COPY ./JWTAuthentication.Domain.Core/*.csproj JWTAuthentication.Domain.Core/
COPY ./JWTAuthentication.Persistence/*.csproj JWTAuthentication.Persistence/
COPY ./JWTAuthentication.Application/*.csproj JWTAuthentication.Application/
COPY ./JWTAuthentication.Application.Test/*.csproj JWTAuthentication.Application.Test/
RUN dotnet restore 
COPY . .

WORKDIR /src/JWTAuthentication.API
RUN dotnet build ./JWTAuthentication.API.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ./JWTAuthentication.API.csproj -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT dotnet JWTAuthentication.API.dll