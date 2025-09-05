# build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
# usa $PORT quando a plataforma definir; 8080 local
CMD ["sh","-c","dotnet BrigadeiroApp.dll --urls http://0.0.0.0:${PORT:-8080}"]
