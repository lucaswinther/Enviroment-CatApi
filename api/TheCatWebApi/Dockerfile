#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TheCatWebApi.csproj", ""]
COPY ["../TheCatsApplication/TheCatsApplication.csproj", "../TheCatsApplication/"]
COPY ["../CatsDomain/TheCatsDomain.csproj", "../CatsDomain/"]
COPY ["../TheCatsRepository/TheCatsRepository.csproj", "../TheCatsRepository/"]
RUN dotnet restore "./TheCatWebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TheCatWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheCatWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheCatWebApi.dll"]