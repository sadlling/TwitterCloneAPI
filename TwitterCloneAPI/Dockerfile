# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TwitterCloneAPI/TwitterCloneAPI.csproj", "TwitterCloneAPI/"]
RUN dotnet restore "./TwitterCloneAPI/./TwitterCloneAPI.csproj"
COPY . .
WORKDIR "/src/TwitterCloneAPI"
RUN dotnet build "./TwitterCloneAPI.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "./TwitterCloneAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Create and trust SSL certificate
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS ssl
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet dev-certs https --clean && dotnet dev-certs https -ep /https/TwitterCloneAPI.pfx -p MySuperStrongPassword123 && dotnet dev-certs https --trust

# Stage 4: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ssl /https/TwitterCloneAPI.pfx /https/TwitterCloneAPI.pfx
ENTRYPOINT ["dotnet", "TwitterCloneAPI.dll"]