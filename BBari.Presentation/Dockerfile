FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY BBari.Presentation/BBari.Presentation.csproj BBari.Presentation/
COPY BBari.Application/BBari.Application.csproj BBari.Application/
COPY BBari.Utils/BBari.Utils.csproj BBari.Utils/
COPY BBari.Domain/BBari.Domain.csproj BBari.Domain/
COPY BBari.EventBus/BBari.EventBus.csproj BBari.EventBus/
RUN dotnet restore BBari.Presentation/BBari.Presentation.csproj
COPY . .
WORKDIR /src/BBari.Presentation
RUN dotnet build BBari.Presentation.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish BBari.Presentation.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BBari.Presentation.dll"]
