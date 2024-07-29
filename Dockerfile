#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClaculatorAPI.csproj", "."]
RUN dotnet restore "./ClaculatorAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ClaculatorAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClaculatorAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClaculatorAPI.dll"]