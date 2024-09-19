#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Lpnet.WebApp/Lpnet.WebApp.csproj", "Lpnet.WebApp/"]
COPY ["Ats.Domain/Ats.Domain.csproj", "Ats.Domain/"]
COPY ["Ats.Commons/Ats.Commons.csproj", "Ats.Commons/"]
COPY ["Ats.Models/Ats.Models.csproj", "Ats.Models/"]
COPY ["Ats.Security/Ats.Security.csproj", "Ats.Security/"]
COPY ["Ats.Data/Ats.Data.csproj", "Ats.Data/"]
COPY ["Ats.Services/Ats.Services.csproj", "Ats.Services/"]
COPY ["Ats.Build/Ats.Build.csproj", "Ats.Build/"]
RUN dotnet restore "Lpnet.WebApp/Lpnet.WebApp.csproj"
COPY . .
WORKDIR "/src/Lpnet.WebApp"
RUN dotnet build "Lpnet.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN mkdir /app/publish
RUN mkdir /app/publish/Vi
RUN dotnet publish "Lpnet.WebApp.csproj" -c Release -o /app/publish
RUN cp -r /app/publish/Vi/* /app/publish/vi

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lpnet.WebApp.dll"]