FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY src/NotificationWorker/NotificationWorker.csproj src/NotificationWorker/

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore src/NotificationWorker/NotificationWorker.csproj --verbosity normal 

COPY src/ src/

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish src/NotificationWorker/NotificationWorker.csproj \
    -c $BUILD_CONFIGURATION -o /app/publish \
    /p:UseAppHost=false
    
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime

ARG APP_USER=app
ARG APP_UID=1000

RUN adduser -S -u ${APP_UID:-1000} -G ${APP_USER} -h /app ${APP_USER} || true

WORKDIR /app

ENV DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build --chown=${APP_USER}:${APP_USER} /app/publish .

USER ${APP_USER}

ENTRYPOINT ["dotnet", "NotificationWorker.dll"]