#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TheradexPortal.csproj", "."]
RUN dotnet restore "./TheradexPortal.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TheradexPortal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheradexPortal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheradexPortal.dll"]