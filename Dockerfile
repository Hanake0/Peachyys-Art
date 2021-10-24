#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Peachyys.csproj", "."]
RUN dotnet restore "./Peachyys.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Peachyys.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Peachyys.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Padrão de container ASP.NET
# ENTRYPOINT ["dotnet", "Peachyys.dll"]

# Opção utilizada pela Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Peachyys.dll
