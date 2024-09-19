#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Ats.Api/Ats.Api.csproj", "Ats.Api/"]
COPY ["Ats.Models/Ats.Models.csproj", "Ats.Models/"]
COPY ["Ats.Domain/Ats.Domain.csproj", "Ats.Domain/"]
COPY ["Ats.Commons/Ats.Commons.csproj", "Ats.Commons/"]
COPY ["Ats.Security/Ats.Security.csproj", "Ats.Security/"]
COPY ["Ats.Data/Ats.Data.csproj", "Ats.Data/"]
COPY ["Ats.Services/Ats.Services.csproj", "Ats.Services/"]
RUN dotnet restore "Ats.Api/Ats.Api.csproj"
COPY . .
WORKDIR "/src/Ats.Api"
RUN dotnet build "Ats.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ats.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ats.Api.dll"]