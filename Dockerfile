FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Datarisk.Api/Datarisk.Api.csproj", "src/Datarisk.Api/"]
COPY ["src/Datarisk.Core/Datarisk.Core.csproj", "src/Datarisk.Core/"]
COPY ["src/Datarisk.Infrastructure/Datarisk.Infrastructure.csproj", "src/Datarisk.Infrastructure/"]
COPY ["src/Datarisk.Application/Datarisk.Application.csproj", "src/Datarisk.Application/"]
RUN dotnet restore "src/Datarisk.Api/Datarisk.Api.csproj"
COPY . .
WORKDIR "/src/src/Datarisk.Api"
RUN dotnet build "Datarisk.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Datarisk.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Datarisk.Api.dll"]
