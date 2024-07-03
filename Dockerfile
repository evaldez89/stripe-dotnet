FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./app .

RUN dotnet restore
RUN dotnet publish -c Release -o out

RUN dotnet tool install --global dotnet-ef --version 8.*

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ARG APP_PORT
ENV APP_PORT=${APP_PORT:-5000}

COPY --from=build /app/out .
COPY set_port.sh set_port.sh

RUN chmod +x set_port.sh
RUN APP_PORT=$(./set_port.sh)

EXPOSE $APP_PORT

ENV ASPNETCORE_URLS=http://0.0.0.0:$APP_PORT

ENTRYPOINT ["dotnet", "app.dll"]