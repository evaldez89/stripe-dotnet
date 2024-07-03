FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./app .

# RUN dotnet install --add-rid linux-x64 Microsoft.AspNetCore.App 8.0.0

RUN dotnet restore
RUN dotnet publish -c Release -o out

# # Temporary stage to install framework
# FROM mcr.microsoft.com/dotnet/runtime:8.0 AS install-runtime
# WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 8.*
# RUN dotnet install --add-rid linux-x64 Microsoft.AspNetCore.App 8.0.0
# Microsoft.AspNetCore.App 8.0.5

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ARG APP_PORT
ENV APP_PORT=${APP_PORT:-5000}

# COPY --from=install-runtime /usr/share/dotnet/shared/Microsoft.AspNetCore.App/8.0.0/ /usr/share/dotnet/shared/Microsoft.AspNetCore.App/8.0.0/
COPY --from=build /app/out .
COPY set_port.sh set_port.sh

RUN chmod +x set_port.sh
RUN APP_PORT=$(./set_port.sh)

EXPOSE $APP_PORT

ENV ASPNETCORE_URLS=http://0.0.0.0:$APP_PORT

ENTRYPOINT ["dotnet", "app.dll"]