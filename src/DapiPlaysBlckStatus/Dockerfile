FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/DapiPlaysBlckStatus/DapiPlaysBlckStatus.csproj", "src/DapiPlaysBlckStatus/"]
RUN dotnet restore "src/DapiPlaysBlckStatus/DapiPlaysBlckStatus.csproj"
COPY . .
WORKDIR "/src/src/DapiPlaysBlckStatus"
RUN dotnet build "DapiPlaysBlckStatus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DapiPlaysBlckStatus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DapiPlaysBlckStatus.dll"]