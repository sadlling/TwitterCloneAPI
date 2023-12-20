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
FROM build AS ssl
WORKDIR /app
COPY --from=publish /app/publish .

# Генерация сертификата
RUN openssl req -x509 -nodes -newkey rsa:2048 -keyout localhost.key -out localhost.crt -subj "/CN=localhost" -days 365

# Конвертация в формат PFX (если нужно)
RUN openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt -passout pass:mypassword

# Перемещение сертификатов в нужные места
RUN mkdir /https
RUN mv localhost.crt /https/localhost.crt
RUN mv localhost.key /https/localhost.key
RUN mv localhost.pfx /https/localhost.pfx

# Stage 4: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ssl /https /https
ENTRYPOINT ["dotnet", "TwitterCloneAPI.dll"]
