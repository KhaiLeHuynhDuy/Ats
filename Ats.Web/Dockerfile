#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

#RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano
#RUN curl -sL https://deb.nodesource.com/setup_8.x | bash - && apt-get install -yq nodejs build-essential
#RUN npm install -g npm
#RUN npm install
RUN apt-get update && apt-get -y install nodejs

WORKDIR /src
COPY ["Ats.Web/Ats.Web.csproj", "Ats.Web/"]
COPY ["Ats.Models/Ats.Models.csproj", "Ats.Models/"]
COPY ["Ats.Domain/Ats.Domain.csproj", "Ats.Domain/"]
COPY ["Ats.Commons/Ats.Commons.csproj", "Ats.Commons/"]
COPY ["Ats.Security/Ats.Security.csproj", "Ats.Security/"]
COPY ["Ats.Data/Ats.Data.csproj", "Ats.Data/"]
COPY ["Ats.Services/Ats.Services.csproj", "Ats.Services/"]
COPY ["Ats.Build/Ats.Build.csproj", "Ats.Build/"]
RUN dotnet restore "Ats.Web/Ats.Web.csproj"
COPY . .
WORKDIR "/src/Ats.Web"
RUN dotnet build "Ats.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN mkdir /app/publish && mkdir /app/publish/Vi
RUN dotnet publish "Ats.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ats.Web.dll"]