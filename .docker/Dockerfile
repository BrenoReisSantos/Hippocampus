ARG PROJECT_NAME="Hippocampus"
ARG CONFIGURATION="release"

FROM mcr.microsoft.com/dotnet/sdk:7.0.101-alpine3.17 as build

ARG PROJECT_NAME
ARG CONFIGURATION

WORKDIR /${PROJECT_NAME}

COPY ./src ./

WORKDIR /${PROJECT_NAME}/${PROJECT_NAME}.Api

RUN dotnet restore
RUN dotnet publish -c ${CONFIGURATION} --self-contained --use-current-runtime -o ./publish

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine3.17 as run

ARG PROJECT_NAME
ENV EXECUTABLE_NAME=${PROJECT_NAME}

WORKDIR /${PROJECT_NAME}

COPY --from=build /${PROJECT_NAME}/${PROJECT_NAME}.Api/publish ./

ENTRYPOINT ./${EXECUTABLE_NAME}.Api
