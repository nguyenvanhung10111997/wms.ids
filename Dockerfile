# Stage 1: Base Image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["wms.ids.web/wwwroot", "wms.ids.web/"]
COPY ["wms.ids.dto/wms.ids.dto.csproj", "wms.ids.dto/"]
COPY ["wms.infrastructure/wms.infrastructure.csproj", "wms.infrastructure/"]
COPY ["wms.ids.business/wms.ids.business.csproj", "wms.ids.business/"]
COPY ["wms.ids.web/wms.ids.web.csproj", "wms.ids.web/"]


# Restore project dependencies
RUN dotnet restore "wms.ids.web/wms.ids.web.csproj"

# Copy the remaining files and build the application
COPY . .
WORKDIR "/src/wms.ids.web"
RUN dotnet build "wms.ids.web.csproj" -c Release -o /app/build

# Stage 3: Publish Image
FROM build AS publish
RUN dotnet publish "wms.ids.web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "wms.ids.web.dll"]